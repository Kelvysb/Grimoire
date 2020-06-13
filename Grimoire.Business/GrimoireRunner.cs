using System;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Events;
using Grimoire.Domain.Models;

namespace Grimoire.Business
{
    public class GrimoireRunner : IGrimoireRunner
    {
        private int timer = 0;

        private IGrimoireBusiness business;

        public event StartHandler Start;

        public event FinishHandler Finish;

        public event TimerHandler Timer;

        public bool Paused { get; set; }

        public GrimoireScriptBlock ScriptBlock { get; private set; }

        public bool Selected { get; set; }

        public bool IsRunning { get; private set; }

        public GrimoireRunner(GrimoireScriptBlock scriptBlock, IGrimoireBusiness business)
        {
            Selected = false;
            Paused = false;
            this.business = business;
            ScriptBlock = scriptBlock;
            Task.Run(() => TimerRun());
            CheckRunOnStart();
        }

        public override string ToString()
        {
            string result = ScriptBlock.Description;

            if (ScriptBlock.ExecutionMode == ExecutionMode.Interval)
            {
                result = $"{result} ({ timer }secs)";
            }

            if (Selected)
            {
                result = $"{result} <-";
            }
            if (IsRunning)
            {
                result = $"RUNNING - {result}";
            }
            else
            {
                switch (ScriptBlock.LastResult?.ResultType)
                {
                    case ResultType.Success:
                        result = $"OK - {result}";
                        break;

                    case ResultType.Error:
                        result = $"ERROR - {result}";
                        break;

                    case ResultType.Warning:
                        result = $"WARNING - {result}";
                        break;
                }
            }
            return result.Trim();
        }

        public async Task<ScriptResult> Run()
        {
            ScriptResult result;

            IsRunning = true;
            Start?.Invoke(ScriptBlock);
            result = await ExecuteScript(ScriptBlock);
            await Task.Run(() =>
            {
                Finish?.Invoke(ScriptBlock, result);
                IsRunning = false;
            });
            return result;
        }

        private async void CheckRunOnStart()
        {
            if (ScriptBlock.ExecutionMode == ExecutionMode.RunOnStart)
            {
                await Run();
            }
        }

        private void TimerRun()
        {
            timer = ScriptBlock.Interval;
            while (ScriptBlock.ExecutionMode == ExecutionMode.Interval)
            {
                if (!Paused)
                {
                    if (timer <= 0)
                    {
                        timer = ScriptBlock.Interval;
                        Run().Wait();
                    }
                    timer--;
                    Timer?.Invoke(ScriptBlock, timer);
                }
                Thread.Sleep(1000);
            }
        }

        private async Task<ScriptResult> ExecuteScript(GrimoireScriptBlock scriptBlock)
        {
            try
            {
                return await RunScriptBlock(scriptBlock);
            }
            catch (Exception ex)
            {
                await CreateErrorResult(scriptBlock, ex);
                throw;
            }
        }

        private async Task CreateErrorResult(GrimoireScriptBlock scriptBlock, Exception ex)
        {
            ScriptResult result = new ScriptResult()
            {
                ResultType = ResultType.Error,
                Errors = ex.Message
            };
            scriptBlock.LastResult = result;
            await business.SaveScriptBlock(scriptBlock);
        }

        private async Task<ScriptResult> RunScriptBlock(GrimoireScriptBlock scriptBlock)
        {
            ScriptResult result = null;
            switch (scriptBlock.ScriptType)
            {
                case ScriptType.PowerShell:
                    result = await ExecutePowerShell(scriptBlock);
                    break;

                case ScriptType.Python:
                    break;

                default:
                    break;
            }
            scriptBlock.LastResult = result;
            await business.SaveScriptBlock(scriptBlock);
            return result;
        }

        private async Task<ScriptResult> ExecutePowerShell(GrimoireScriptBlock scriptBlock)
        {
            ScriptResult result = null;
            string script = await business.ReadScript(scriptBlock);
            using (PowerShell ps = PowerShell.Create())
            {
                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                ps.AddScript("Set-ExecutionPolicy -Scope Process Unrestricted");
                ps.AddScript(script);
                IAsyncResult execResult = await Task.Run(() =>
                    ps.BeginInvoke<PSObject, PSObject>(null, outputCollection));
                WaitExecution(scriptBlock.TimeOut, execResult);
                result = GetResult(scriptBlock, ps);
            }
            return result;
        }

        private void WaitExecution(int timeOut, IAsyncResult execResult)
        {
            int time = 0;
            while (!execResult.IsCompleted)
            {
                Thread.Sleep(100);
                time++;
                if (time > (timeOut * 10))
                {
                    throw new TimeoutException();
                }
            }
        }

        private ScriptResult GetResult(GrimoireScriptBlock scriptBlock, PowerShell ps)
        {
            ScriptResult result = MountResults(ps);
            result.ResultType = GetResultStatus(scriptBlock, ps, result.RawResult);
            result.FilteredResult = ExtractFilteredResult(scriptBlock, result.RawResult);
            return result;
        }

        private ScriptResult MountResults(PowerShell ps)
        {
            return new ScriptResult()
            {
                ResultType = ResultType.None,
                RawResult = String.Join('\n', ps.Streams.Information.Select(r => r.ToString()).ToList()),
                Warninings = String.Join('\n', ps.Streams.Warning.Select(r => r.ToString()).ToList()),
                Errors = String.Join('\n', ps.Streams.Error.Select(r => r.ToString()).ToList())
            };
        }

        private string ExtractFilteredResult(GrimoireScriptBlock scriptBlock, string rawResult)
        {
            string result = rawResult;
            if (scriptBlock.ExtractResult != null && !String.IsNullOrEmpty(scriptBlock.ExtractResult.Start) && !String.IsNullOrEmpty(scriptBlock.ExtractResult.End))
            {
                result = FindResultMatch(scriptBlock, rawResult);
            }
            return result;
        }

        private string FindResultMatch(GrimoireScriptBlock scriptBlock, string rawResult)
        {
            string result;
            Match match = Regex.Match(rawResult, scriptBlock.ExtractResult.Start);
            int startIndex = match.Index + match.Length;
            match = Regex.Match(rawResult, scriptBlock.ExtractResult.End);
            int endIndex = match.Index;
            result = rawResult.Substring(startIndex, endIndex - startIndex);
            return result;
        }

        private ResultType GetResultStatus(GrimoireScriptBlock scriptBlock, PowerShell ps, string rawResult)
        {
            ResultType result = ResultType.None;
            if (String.IsNullOrEmpty(scriptBlock.SuccessPatern) && ps.Streams.Error.Any() && scriptBlock.AlertLevel != AlertLevel.Silent)
            {
                result = scriptBlock.AlertLevel == AlertLevel.Error ? ResultType.Error : ResultType.Warning;
            }
            else if (String.IsNullOrEmpty(scriptBlock.SuccessPatern) && ps.Streams.Warning.Any() && scriptBlock.AlertLevel != AlertLevel.Silent)
            {
                result = ResultType.Warning;
            }
            else
            {
                if (Regex.IsMatch(rawResult, scriptBlock.SuccessPatern))
                {
                    result = ResultType.Success;
                }
                else if (scriptBlock.AlertLevel != AlertLevel.Silent)
                {
                    result = scriptBlock.AlertLevel == AlertLevel.Error ? ResultType.Error : ResultType.Warning;
                }
            }
            return result;
        }
    }
}
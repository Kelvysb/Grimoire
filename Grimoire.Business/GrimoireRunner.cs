﻿using System;
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
        public GrimoireScriptBlock ScriptBlock { get; private set; }

        private IGrimoireBusiness business;

        public event StartHandler Start;

        public event FinishHandler Finish;

        public event TimerHandler Timer;

        public bool Selected { get; set; }

        public bool IsRunning { get; private set; }

        public GrimoireRunner(GrimoireScriptBlock scriptBlock, IGrimoireBusiness business)
        {
            Selected = false;
            this.business = business;
            this.ScriptBlock = scriptBlock;
            Task.Run(() => TimerRun());
        }

        public override string ToString()
        {
            string result = ScriptBlock.Description;

            if (Selected)
            {
                result = $"({result})";
            }
            if (IsRunning)
            {
                result = $"{result} >>>";
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

        private void TimerRun()
        {
            int timer = ScriptBlock.Interval;
            while (ScriptBlock.ExecutionMode == ExecutionMode.Interval)
            {
                if (timer <= 0)
                {
                    timer = ScriptBlock.Interval;
                    Run().Wait();
                }
                Thread.Sleep(1000);
                timer--;
                Timer?.Invoke(ScriptBlock, timer);
            }
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

        private async Task<ScriptResult> ExecuteScript(GrimoireScriptBlock scriptBlock)
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
            string scriptPath = await business.getScriptFullPath(scriptBlock);
            using (PowerShell ps = PowerShell.Create())
            {
                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                ps.AddScript("Set-ExecutionPolicy -Scope Process Unrestricted");
                ps.AddScript($"set-location -path \"{scriptPath}\"");
                ps.AddScript($"./{scriptBlock.Script}");
                IAsyncResult execResult = await Task.Run(() => ps.BeginInvoke<PSObject, PSObject>(null, outputCollection));
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
            string result = "";
            if (scriptBlock.ExtractResult != null && !String.IsNullOrEmpty(scriptBlock.ExtractResult.Start) && !String.IsNullOrEmpty(scriptBlock.ExtractResult.End))
            {
                Match match = Regex.Match(rawResult, scriptBlock.ExtractResult.Start);
                int startIndex = match.Index + match.Length;
                match = Regex.Match(rawResult, scriptBlock.ExtractResult.End);
                int endIndex = match.Index;
                result = rawResult.Substring(startIndex, endIndex - startIndex);
            }
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
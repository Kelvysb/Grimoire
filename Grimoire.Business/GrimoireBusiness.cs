using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Grimoire.Business
{
    public class GrimoireBusiness : IGrimoireBusiness
    {
        private readonly IGrimoireService grimoireService;

        public GrimoireBusiness(IGrimoireService grimoireService)
        {
            this.grimoireService = grimoireService;
        }

        public async Task<ScriptResult> ExecuteScript(GrimoireScriptBlock scriptBlock)
        {
            ScriptResult result = null;
            switch (scriptBlock.ScriptType)
            {
                case ScriptType.PowerShell:
                    result = await Task.Run(() => ExecutePowerShell(scriptBlock));
                    break;

                case ScriptType.Bash:
                    break;

                case ScriptType.Bat:
                    break;

                default:
                    break;
            }
            return result;
        }

        private async Task<ScriptResult> ExecutePowerShell(GrimoireScriptBlock scriptBlock)
        {
            ScriptResult result = null;
            using (PowerShell ps = PowerShell.Create())
            {
                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                ps.AddScript("Set-ExecutionPolicy -Scope Process Unrestricted");
                ps.AddScript(grimoireService.getScriptFullPath(scriptBlock));
                IAsyncResult execResult = await Task.Run(() => ps.BeginInvoke<PSObject, PSObject>(null, outputCollection));
                WaitExecution(scriptBlock.TimeOut, execResult);
                result = GetResult(scriptBlock, ps);
            }
            return result;
        }

        public async Task<List<GrimoireScriptBlock>> GetScriptBlocks()
        {
            return await Task.Run(() => grimoireService.GetScriptBlocks().ToList());
        }

        public async Task SaveScriptBlock(GrimoireScriptBlock scriptBlock)
        {
            await Task.Run(() => grimoireService.SaveScriptBlocks(scriptBlock));
        }

        public async Task RemoveScriptBlock(string name)
        {
            await Task.Run(() => grimoireService.RemoveScriptBlock(name));
        }

        public async Task ChangeScripBlockOrder(GrimoireScriptBlock scriptBlock, int orderShift)
        {
            await Task.Run(() => "");
        }

        public async Task<List<ExecutionGroup>> GetExecutionGroups()
        {
            return await Task.Run(() => grimoireService.GetExecutionGroups().ToList());
        }

        public async Task RemoveExecutionGroup(string name)
        {
            await Task.Run(() => grimoireService.RemoveExecutionGroup(name));
        }

        public async Task SaveExecutionGroup(ExecutionGroup executionGroup)
        {
            await Task.Run(() => grimoireService.SaveExecutionGroup(executionGroup));
        }

        private ScriptResult GetResult(GrimoireScriptBlock scriptBlock, PowerShell ps)
        {
            ScriptResult result = new ScriptResult();
            result.ResultType = ResultType.None;
            result.RawResult = String.Join('\n', ps.Streams.Information.Select(r => r.ToString()).ToList());
            result.Warninings = String.Join('\n', ps.Streams.Warning.Select(r => r.ToString()).ToList());
            result.Errors = String.Join('\n', ps.Streams.Error.Select(r => r.ToString()).ToList());
            result.ResultType = GetResultStatus(scriptBlock, ps, result.RawResult);
            result.FilteredResult = ExtractFilteredResult(scriptBlock, result.RawResult);
            return result;
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
    }
}
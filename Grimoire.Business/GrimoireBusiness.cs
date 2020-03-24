using System.Collections.Generic;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Business
{
    public class GrimoireBusiness : IGrimoireBusiness
    {
        private readonly IGrimoireService grimoireService;

        public GrimoireBusiness(IGrimoireService grimoireService)
        {
            this.grimoireService = grimoireService;
        }

        public ICollection<string> ExecuteScript(ScriptBlock scriptBlock)
        {
            return new List<string>();
        }

        public ICollection<ScriptBlock> GetScriptBlocks()
        {
            return grimoireService.GetScriptBlocks();
        }

        public void SaveScripBlock(ScriptBlock scriptBlock)
        {
            grimoireService.SaveScriptBlocks(scriptBlock);
        }

        public void RemoveScriptBlock(string name)
        {
            grimoireService.RemoveScriptBlock(name);
        }

        public void ChangeScripBlockOrder(ScriptBlock scriptBlock, int orderShift)
        {
        }

        public ICollection<ExecutionGroup> GetExecutionGroups()
        {
            return grimoireService.GetExecutionGroups();
        }

        public void RemoveExecutionGroup(string name)
        {
            grimoireService.RemoveExecutionGroup(name);
        }

        public void SaveExecutionGroup(ExecutionGroup executionGroup)
        {
            grimoireService.SaveExecutionGroup(executionGroup);
        }
    }
}
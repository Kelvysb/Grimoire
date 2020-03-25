using System.Collections.Generic;
using System.Linq;
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

        public List<string> ExecuteScript(ScriptBlock scriptBlock)
        {
            return new List<string>();
        }

        public List<ScriptBlock> GetScriptBlocks()
        {
            return grimoireService.GetScriptBlocks().ToList();
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

        public List<ExecutionGroup> GetExecutionGroups()
        {
            return grimoireService.GetExecutionGroups().ToList();
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
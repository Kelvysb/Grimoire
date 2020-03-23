using System.Collections.Generic;
using Grimoire.Models;
using Grimoire.Services;

namespace Grimoire.Business
{
    public class GrimoireBusiness
    {
        private readonly IConfigurationService configurationService;
        private readonly IGrimoireService grimoireService;

        public GrimoireBusiness(IConfigurationService configurationService,
                                IGrimoireService grimoireService)
        {
            this.configurationService = configurationService;
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
    }
}
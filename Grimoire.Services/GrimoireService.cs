using System.Collections.Generic;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Services
{
    public class GrimoireService : IGrimoireService
    {
        private readonly IConfigurationService configurationService;

        public GrimoireService(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public ICollection<ScriptBlock> GetScriptBlocks()
        {
            return new List<ScriptBlock>()
            {
                new ScriptBlock()
                {
                    Group = "Test",
                    Order = 1,
                    Name = "Test",
                    Description = "Test Script",
                    AlertLevel = AlertLevel.Warning,
                    ScriptType = ScriptType.PowerShell,
                    Interval = 15,
                    SuccessPatern = ".*",
                    File = "test.ps1"
                },
                new ScriptBlock()
                {
                    Group = "Test",
                    Order = 2,
                    Name = "Test second",
                    Description = "Test Script",
                    AlertLevel = AlertLevel.Warning,
                    ScriptType = ScriptType.PowerShell,
                    Interval = 15,
                    SuccessPatern = ".*",
                    File = "test.ps1"
                },
                new ScriptBlock()
                {
                    Group = "Test",
                    Order = 2,
                    Name = "Test third",
                    Description = "Test Script",
                    AlertLevel = AlertLevel.Warning,
                    ScriptType = ScriptType.PowerShell,
                    Interval = 15,
                    SuccessPatern = ".*",
                    File = "test.ps1"
                }
            };
        }

        public void RemoveScriptBlock(string name)
        {
        }

        public void SaveScriptBlocks(ScriptBlock scriptBlock)
        {
            throw new System.NotImplementedException();
        }
    }
}
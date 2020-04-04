using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages
{
    public class ScriptEditBase : ComponentBase
    {
        [Inject]
        private IGrimoireBusiness grimoireBusiness { get; set; }

        [Parameter]
        public string ScriptName { get; set; }

        public GrimoireScriptBlock ScriptBlock { get; set; }

        protected async override Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(ScriptName))
            {
                ScriptBlock = await grimoireBusiness.GetScriptBlock(ScriptName);
            }
            else
            {
                ScriptBlock = new GrimoireScriptBlock()
                {
                    Name = "",
                    Description = "",
                    AlertLevel = AlertLevel.Error,
                    ExtractResult = new PatternRange() { Start = "", End = "" },
                    Group = "",
                    Interval = 0,
                    Order = 0,
                    OriginalScriptPath = "",
                    Script = "",
                    ScriptBlocks = null,
                    ScriptType = ScriptType.PowerShell,
                    SuccessPatern = "",
                    TimeOut = 10
                };
            }
            await base.OnInitializedAsync();
        }

        public Task Submit()
        {
            return Task.Run(() => "");
        }
    }
}
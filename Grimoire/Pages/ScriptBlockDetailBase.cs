using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages
{
    public class ScriptBlockDetailBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness grimoireBusiness { get; set; }

        [Parameter]
        public string ScriptName { get; set; }

        public GrimoireScriptBlock ScriptBlock { get; set; }

        public bool Executing { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Executing = false;
            ScriptBlock = await grimoireBusiness.GetScriptBlock(ScriptName);
            await base.OnInitializedAsync();
        }

        public Task EditScript()
        {
            return Task.Run(() => "");
        }

        public async Task RunScript()
        {
            Executing = true;
            //ScriptBlock.LastResult = await grimoireBusiness.ExecuteScript(ScriptBlock);
            await Task.Run(() => Executing = false);
        }

        public Task RemoveScript()
        {
            return Task.Run(() => "");
        }
    }
}
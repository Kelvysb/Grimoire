using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class ScriptBlockItemBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness grimoireBusiness { get; set; }

        [Parameter]
        public GrimoireScriptBlock ScriptBlock { get; set; }

        public bool Executing { get; set; } = false;

        public Task EditScript()
        {
            return Task.Run(() => "");
        }

        public async Task RunScript()
        {
            Executing = true;
            ScriptBlock.LastResult = await grimoireBusiness.ExecuteScript(ScriptBlock);
            await Task.Run(() => Executing = false);
        }
    }
}
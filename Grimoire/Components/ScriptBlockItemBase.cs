using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class ScriptBlockItemBase : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public IGrimoireRunner ScriptBlockRunner { get; set; }

        public string Info { get; set; }

        protected override Task OnInitializedAsync()
        {
            if (ScriptBlockRunner.ScriptBlock.ExecutionMode == ExecutionMode.Interval
                && ScriptBlockRunner.ScriptBlock.Interval > 0)
            {
                Info = $"Interval ({ScriptBlockRunner.ScriptBlock.Interval}): {ScriptBlockRunner.ScriptBlock.Interval}";
            }
            else if (ScriptBlockRunner.ScriptBlock.ExecutionMode == ExecutionMode.RunOnStart)
            {
                Info = $"Run on start";
            }
            else
            {
                Info = $"Manual";
            }
            return base.OnInitializedAsync();
        }

        public async void RunScript()
        {
            if (!ScriptBlockRunner.IsRunning)
            {
                await ScriptBlockRunner.Run();
            }
        }

        public void EditScript()
        {
            if (!ScriptBlockRunner.IsRunning)
            {
                NavigationManager.NavigateTo($"Detail/{ScriptBlockRunner.ScriptBlock.Name}");
            }
        }
    }
}
using System.Threading;
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

        [Inject]
        public IGrimoireBusiness grimoireBusiness { get; set; }

        [Parameter]
        public GrimoireScriptBlock ScriptBlock { get; set; }

        public string Info { get; set; }

        public bool Executing { get; set; } = false;

        private Task AutoExecutionTask;

        protected override Task OnInitializedAsync()
        {
            if (ScriptBlock.ExecutionMode == ExecutionMode.Interval
                && ScriptBlock.Interval > 0)
            {
                AutoExecutionTask = AutoExecuteTimer();
                Info = $"Interval ({ScriptBlock.Interval}): {ScriptBlock.Interval}";
            }
            else if (ScriptBlock.ExecutionMode == ExecutionMode.RunOnStart)
            {
                Info = $"Run on start";
            }
            else
            {
                Info = $"Manual";
            }
            return base.OnInitializedAsync();
        }

        public void EditScript()
        {
            if (!Executing)
            {
                NavigationManager.NavigateTo($"Detail/{ScriptBlock.Name}");
            }
        }

        public async Task RunScript()
        {
            Executing = true;
            ScriptBlock.LastResult = await grimoireBusiness.ExecuteScript(ScriptBlock);
            await Task.Run(() => Executing = false);
        }

        private async Task AutoExecuteTimer()
        {
            int count = 0;
            do
            {
                if (!Executing)
                {
                    if (count >= ScriptBlock.Interval)
                    {
                        count = 0;
                        await RunScript();
                    }
                    count++;
                    Info = $"Interval ({ScriptBlock.Interval}): {ScriptBlock.Interval - count}";
                    Thread.Sleep(1000);
                }
            } while (true);
        }
    }
}
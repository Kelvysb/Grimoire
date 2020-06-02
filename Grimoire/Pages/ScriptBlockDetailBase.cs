using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Grimoire.Pages
{
    public class ScriptBlockDetailBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness grimoireBusiness { get; set; }

        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        public IGrimoireRunner ScriptBlockRunner { get; set; }

        public bool Executing { get; set; }

        public delegate void EditEventHandler(IGrimoireRunner scriptRunner);

        public event EditEventHandler Edit;

        public delegate void DeleteEventHandler(IGrimoireRunner scriptRunner);

        public event DeleteEventHandler Delete;

        protected override async Task OnInitializedAsync()
        {
            Executing = false;
            await base.OnInitializedAsync();
        }

        public Task EditScript()
        {
            return Task.Run(() => Edit?.Invoke(ScriptBlockRunner));
        }

        public async Task RunScript()
        {
            Executing = true;
            ScriptBlockRunner.ScriptBlock.LastResult = await ScriptBlockRunner.Run();
            await Task.Run(() => Executing = false);
        }

        public async Task RemoveScript()
        {
            if (await jsRuntime.InvokeAsync<bool>("Confirm", "Are you sure?"))
            {
                await grimoireBusiness.RemoveScriptBlock(ScriptBlockRunner.ScriptBlock.Name);
                await jsRuntime.InvokeAsync<object>("Alert", "Script deleted");
                await Task.Run(() => Delete?.Invoke(ScriptBlockRunner));
            }
        }

        public void Reload()
        {
            StateHasChanged();
        }
    }
}
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class SideMenuBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness Business { get; set; }

        public ScriptBlockItem ItemRef { set { AddItem(value); } }

        public delegate void SelectHandler(IGrimoireRunner scriptRunner);

        public event SelectHandler Select;

        public delegate void NewScriptHandler();

        public event NewScriptHandler NewScript;

        protected override Task OnInitializedAsync()
        {
            if (Business.ScriptRunners == null)
            {
                Business.LoadScriptRunners();
            }

            return base.OnInitializedAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Task.Run(() => UpdateTimer());
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        public void AddItem(ScriptBlockItemBase item)
        {
            item.Select += SelectItem;
        }

        public void AddNewScript()
        {
            NewScript?.Invoke();
        }

        public void Reload()
        {
            Business.LoadScriptRunners();
        }

        private void SelectItem(IGrimoireRunner scriptRunner)
        {
            Select?.Invoke(scriptRunner);
        }

        private async void UpdateTimer()
        {
            do
            {
                await InvokeAsync(() => StateHasChanged());
                await Task.Delay(1000);
            } while (true);
        }
    }
}
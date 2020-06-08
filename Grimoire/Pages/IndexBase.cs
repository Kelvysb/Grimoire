using System.Threading.Tasks;
using Grimoire.Components;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Shared;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness grimoireBusiness { get; set; }

        public SideMenuBase SideMenu { get; set; }

        public ScriptBlockDetailBase ScriptDetail { get; set; }

        public ScriptEditBase ScriptEdit { get; set; }

        public ConfigBase Config { get; set; }

        public bool ConfirmEditModal { get; set; }

        public bool ConfirmDeleteModal { get; set; }

        public bool ConfirmConfigModal { get; set; }

        public AppMode AppMode { get; set; }

        private IGrimoireRunner selectedScript;

        protected override async Task OnInitializedAsync()
        {
            CloseModals();
            AppMode = AppMode.None;
            await base.OnInitializedAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                SideMenu.Select += SelectScript;
                SideMenu.NewScript += NewScript;
                SideMenu.OpenAbout += About;
                SideMenu.OpenConfig += OpenConfig;
            }
            if (ScriptDetail != null)
            {
                ScriptDetail.ScriptBlockRunner = selectedScript;
                ScriptDetail.Edit += EditScript;
                ScriptDetail.Delete += DeleteScript;
                ScriptDetail.Reload();
            }
            if (ScriptEdit != null)
            {
                ScriptEdit.ScriptName = selectedScript != null ? selectedScript.ScriptBlock.Name : null;
                ScriptEdit.EndEdit += EndEdit;
                ScriptEdit.Reload();
            }
            if (Config != null)
            {
                Config.EndConfig += EndConfig;
                Config.Reload();
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        public void CloseModals()
        {
            ConfirmEditModal = false;
            ConfirmDeleteModal = false;
            ConfirmConfigModal = false;
        }

        public void About()
        {
            ClearEvents();
            AppMode = AppMode.About;
            InvokeAsync(() => StateHasChanged());
        }

        public void OpenConfig()
        {
            ClearEvents();
            AppMode = AppMode.Config;
            InvokeAsync(() => StateHasChanged());
        }

        private void SelectScript(IGrimoireRunner scriptRunner)
        {
            ClearEvents();
            AppMode = AppMode.Detail;
            selectedScript = scriptRunner;
            InvokeAsync(() => StateHasChanged());
        }

        private void EditScript(IGrimoireRunner scriptRunner)
        {
            ClearEvents();
            AppMode = AppMode.Edit;
            scriptRunner.Paused = true;
            selectedScript = scriptRunner;
            InvokeAsync(() => StateHasChanged());
        }

        private void NewScript()
        {
            ClearEvents();
            AppMode = AppMode.Edit;
            selectedScript = null;
            InvokeAsync(() => StateHasChanged());
        }

        private void EndEdit()
        {
            ClearEvents();
            AppMode = AppMode.None;
            SideMenu.Reload();
            ConfirmEditModal = true;
            InvokeAsync(() => StateHasChanged());
        }

        private void EndConfig(bool changed)
        {
            ClearEvents();
            AppMode = AppMode.None;
            if (changed)
            {
                ConfirmConfigModal = true;
            }
            InvokeAsync(() => StateHasChanged());
        }

        public void ConfirmReload()
        {
            CloseModals();
            InvokeAsync(() => StateHasChanged());
        }

        private void DeleteScript(IGrimoireRunner scriptRunner)
        {
            ClearEvents();
            AppMode = AppMode.None;
            SideMenu.Reload();
            ConfirmDeleteModal = true;
            InvokeAsync(() => StateHasChanged());
        }

        private void ClearEvents()
        {
            if (ScriptDetail != null)
            {
                ScriptDetail.Edit -= EditScript;
                ScriptDetail.Delete -= DeleteScript;
            }
            if (ScriptEdit != null)
            {
                ScriptEdit.EndEdit -= EndEdit;
            }
            if (Config != null)
            {
                Config.EndConfig -= EndConfig;
            }
        }
    }
}
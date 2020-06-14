using System;
using System.Threading.Tasks;
using Grimoire.Components;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Shared;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness Business { get; set; }

        [Inject]
        public ILogService LogService { get; set; }

        public SideMenuBase SideMenu { get; set; }

        public ScriptBlockDetailBase ScriptDetail { get; set; }

        public ScriptEditBase ScriptEdit { get; set; }

        public ConfigBase Config { get; set; }

        public bool ConfirmEditModal { get; set; }

        public bool ConfirmDeleteModal { get; set; }

        public bool ConfirmConfigModal { get; set; }

        public bool ErrorModal { get; set; }

        public string LastError { get; set; }

        public string LastErrorDetail { get; set; }

        public AppMode AppMode { get; set; }

        public string PinInput { get; set; } = "";

        public bool ConfirmResetVault { get; set; } = false;

        private IGrimoireRunner selectedScript;

        protected override async Task OnInitializedAsync()
        {
            CloseModals();
            AppMode = AppMode.None;
            await Business.GetConfig();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                SideMenu.Select += SelectScript;
                SideMenu.NewScript += NewScript;
                SideMenu.OpenAbout += About;
                SideMenu.OpenConfig += OpenConfig;
                SideMenu.OpenVault += OpenVault;
                SideMenu.Error += ShowError;
            }
            if (ScriptDetail != null)
            {
                ScriptDetail.ScriptBlockRunner = selectedScript;
                ScriptDetail.Edit += EditScript;
                ScriptDetail.Delete += DeleteScript;
                ScriptDetail.Error += ShowError;
                ScriptDetail.Reload();
            }
            if (ScriptEdit != null)
            {
                ScriptEdit.ScriptName = selectedScript != null ? selectedScript.ScriptBlock.Name : null;
                ScriptEdit.EndEdit += EndEdit;
                ScriptEdit.Error += ShowError;
                ScriptEdit.Reload();
            }
            if (Config != null)
            {
                Config.EndConfig += EndConfig;
                Config.Reload();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public void CloseModals()
        {
            ConfirmEditModal = false;
            ConfirmDeleteModal = false;
            ConfirmConfigModal = false;
            ErrorModal = false;
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

        public void OpenVault()
        {
            ClearEvents();
            AppMode = AppMode.Vault;
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

        private async Task EndEdit()
        {
            ClearEvents();
            AppMode = AppMode.None;
            await SideMenu.Reload();
            ConfirmEditModal = true;
            await InvokeAsync(() => StateHasChanged());
        }

        private async Task EndConfig(bool changed)
        {
            ClearEvents();
            AppMode = AppMode.None;
            if (changed)
            {
                await Business.GetConfig();
                ConfirmConfigModal = true;
            }
            await InvokeAsync(() => StateHasChanged());
        }

        public void ConfirmReload()
        {
            CloseModals();
            InvokeAsync(() => StateHasChanged());
        }

        private async Task DeleteScript(IGrimoireRunner scriptRunner)
        {
            ClearEvents();
            AppMode = AppMode.None;
            await SideMenu.Reload();
            ConfirmDeleteModal = true;
            await InvokeAsync(() => StateHasChanged());
        }

        private async Task ShowError(string message, Exception ex)
        {
            LastError = message;
            LastErrorDetail = ex.Message;
            LogService.Log(ex);
            ErrorModal = true;
            AppMode = AppMode.None;
            await SideMenu.Reload();
            await InvokeAsync(() => StateHasChanged());
        }

        private void ClearEvents()
        {
            if (ScriptDetail != null)
            {
                ScriptDetail.Edit -= EditScript;
                ScriptDetail.Delete -= DeleteScript;
                ScriptDetail.Error -= ShowError;
            }
            if (ScriptEdit != null)
            {
                ScriptEdit.EndEdit -= EndEdit;
                ScriptEdit.Error -= ShowError;
            }
            if (Config != null)
            {
                Config.EndConfig -= EndConfig;
            }
        }
    }
}
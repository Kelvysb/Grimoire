using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool PinModal { get; set; } = false;

        public bool LoginError { get; set; } = false;

        public List<string> Groups { get; set; }

        public delegate void SelectHandler(IGrimoireRunner scriptRunner);

        public event SelectHandler Select;

        public delegate void NewScriptHandler();

        public event NewScriptHandler NewScript;

        public delegate void AboutHandler();

        public AboutHandler OpenAbout;

        public delegate void ConfigHandler();

        public ConfigHandler OpenConfig;

        public delegate void VaultHandler();

        public VaultHandler OpenVault;

        public delegate Task ErrorEventHandler(string message, Exception ex);

        public event ErrorEventHandler Error;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !Business.IsDefaultPin)
            {
                PinModal = true;
                StateHasChanged();
            }
            else if (firstRender)
            {
                await Business.LoadVault();
                await Reload();
            }
            if (firstRender)
            {
                await Task.Run(() => UpdateTimer());
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public void AddItem(ScriptBlockItemBase item)
        {
            item.Select += SelectItem;
        }

        public void AddNewScript()
        {
            NewScript?.Invoke();
        }

        public async Task Reload()
        {
            try
            {
                await Business.LoadScriptRunners();
                await Task.Run(() => Groups = Business.ScriptRunners
                                        .Select(runner => runner.ScriptBlock.Group)
                                        .Distinct()
                                        .OrderBy(group => group)
                                        .ToList());
            }
            catch (Exception ex)
            {
                Error?.Invoke("Error loading scripts.", ex);
            }
        }

        public async void Login(string pin, Action invalidPin)
        {
            if (await Business.CheckPin(pin))
            {
                await Business.LoadVault();
                await Reload();
                PinModal = false;
            }
            else
            {
                invalidPin();
            }
            await InvokeAsync(() => StateHasChanged());
        }

        public async void ResetVault()
        {
            await Business.ResetVault();
            PinModal = false;
            await Reload();
            await InvokeAsync(() => StateHasChanged());
        }

        public void About()
        {
            OpenAbout?.Invoke();
        }

        public void Config()
        {
            OpenConfig?.Invoke();
        }

        public void Vault()
        {
            OpenVault?.Invoke();
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
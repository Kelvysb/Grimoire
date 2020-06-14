using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class ConfigBase : ComponentBase
    {
        [Inject]
        private IGrimoireBusiness grimoireBusiness { get; set; }

        public GrimoireConfig config { get; set; } = new GrimoireConfig();

        public delegate Task EndConfigHandler(bool changed);

        public event EndConfigHandler EndConfig;

        protected override async Task OnInitializedAsync()
        {
            await LoadConfig();
            await base.OnInitializedAsync();
        }

        private async Task LoadConfig()
        {
            config = await grimoireBusiness.GetConfig();
            StateHasChanged();
        }

        public async Task Save()
        {
            await grimoireBusiness.SaveConfig(config);
            await Task.Run(() => EndConfig?.Invoke(true));
        }

        public async Task Close()
        {
            await Task.Run(() => EndConfig?.Invoke(false));
        }

        public async void Reload()
        {
            await LoadConfig();
            StateHasChanged();
        }
    }
}
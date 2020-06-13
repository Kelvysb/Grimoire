using System.Linq;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class VaultEditBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness Business { get; set; }

        public VaultItem NewVaultItem { get; set; } = new VaultItem();

        public string NewPin { get; set; } = "";

        public string OldPin { get; set; } = "";

        public bool InvalidPin { get; set; } = false;

        public bool KeyExistsModal { get; set; } = false;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        public async Task AddItem()
        {
            if (!Business.Vault.Itens.Any(item =>
                item.Key.Equals(NewVaultItem.Key,
                System.StringComparison.InvariantCultureIgnoreCase)))
            {
                Business.Vault.Itens.Add(NewVaultItem);
                await Business.SaveVault();
                NewVaultItem = new VaultItem();
                await InvokeAsync(() => StateHasChanged());
            }
            else
            {
                KeyExistsModal = true;
            }
        }

        public async Task SetPin()
        {
            if (!string.IsNullOrEmpty(NewPin.Trim()))
            {
                InvalidPin = !await Business.ChangePin(OldPin, NewPin.Trim());
                NewPin = "";
                OldPin = "";
                await InvokeAsync(() => StateHasChanged());
            }
            else
            {
                InvalidPin = true;
            }
        }

        public async Task ClearPin()
        {
            InvalidPin = !await Business.ClearPin(OldPin);
            NewPin = "";
            OldPin = "";
            await InvokeAsync(() => StateHasChanged());
        }

        public void UpdateState()
        {
            InvokeAsync(() => StateHasChanged());
        }
    }
}
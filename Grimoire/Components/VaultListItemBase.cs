using System;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class VaultListItemBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness GrimoireBusiness { get; set; }

        [Parameter]
        public VaultItem VaultItem { get; set; }

        [Parameter]
        public Action UpdateState { get; set; }

        public bool Modified { get; set; } = false;

        private string oldValue = "";

        protected override void OnInitialized()
        {
            oldValue = VaultItem.Value;
            base.OnInitialized();
        }

        public void SetModified()
        {
            if (!oldValue.Equals(VaultItem.Value))
            {
                Modified = true;
                oldValue = VaultItem.Value;
                StateHasChanged();
            }
        }

        public async Task Save()
        {
            await GrimoireBusiness.SaveVault();
            Modified = false;
            await InvokeAsync(() => StateHasChanged());
            UpdateState?.Invoke();
        }

        public async Task Delete()
        {
            GrimoireBusiness.Vault.Itens.Remove(VaultItem);
            await GrimoireBusiness.SaveVault();
            await Save();
        }
    }
}
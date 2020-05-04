using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness grimoireBusiness { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await grimoireBusiness.LoadScriptRunners();
            await base.OnInitializedAsync();
        }
    }
}
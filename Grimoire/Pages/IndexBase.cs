using System.Collections.Generic;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private IGrimoireBusiness grimoireBusiness { get; set; }

        protected List<GrimoireScriptBlock> scripts;

        protected override async Task OnInitializedAsync()
        {
            scripts = await grimoireBusiness.GetScriptBlocks();
            await base.OnInitializedAsync();
        }
    }
}
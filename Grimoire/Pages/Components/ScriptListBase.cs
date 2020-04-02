using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Grimoire.Pages.Components
{
    public class ScriptListBase : ComponentBase
    {
        [Inject]
        private IGrimoireBusiness grimoireBusiness { get; set; }

        protected List<GrimoireScriptBlock> scripts;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            LoadScripts();
        }

        private void LoadScripts()
        {
            scripts = grimoireBusiness.GetScriptBlocks().Result;
        }
    }
}
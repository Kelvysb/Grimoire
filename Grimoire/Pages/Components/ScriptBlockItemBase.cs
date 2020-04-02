using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages.Components
{
    public class ScriptBlockItemBase : ComponentBase
    {
        [Parameter]
        public GrimoireScriptBlock ScriptBlock { get; set; }
    }
}
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class ScriptBlockItemBase : ComponentBase
    {
        [Parameter]
        public GrimoireScriptBlock ScriptBlock { get; set; }
    }
}
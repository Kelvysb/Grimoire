using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages.Components
{
    public class ScriptBlockItemBase : ComponentBase
    {
        [Parameter]
        public ScriptBlock ScriptBlock { get; set; }
    }
}
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class ScriptBlockItemBase : ComponentBase
    {
        [Parameter]
        public IGrimoireRunner ScriptBlockRunner { get; set; }

        public delegate void SelectHandler(IGrimoireRunner scriptRunner);

        public event SelectHandler Select;

        protected override Task OnInitializedAsync()
        {
            ScriptBlockRunner.Start += (object sender) => InvokeAsync(() => StateHasChanged());
            ScriptBlockRunner.Finish += (object sender, ScriptResult result) => InvokeAsync(() => StateHasChanged());
            return base.OnInitializedAsync();
        }

        public void SelectScript()
        {
            Select?.Invoke(ScriptBlockRunner);
        }
    }
}
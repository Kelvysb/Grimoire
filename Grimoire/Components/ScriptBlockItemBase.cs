using System;
using System.Collections.Generic;
using System.Linq;
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

        [Parameter]
        public Action<IList<Input>, Action<IList<Input>>> RequestInputs { get; set; }

        public delegate void SelectHandler(IGrimoireRunner scriptRunner);

        public event SelectHandler Select;

        private IList<Input> Inputs { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ScriptBlockRunner.Start += (object sender) => InvokeAsync(() => StateHasChanged());
            ScriptBlockRunner.Finish += (object sender, ScriptResult result) => InvokeAsync(() => StateHasChanged());
            Inputs = await ScriptBlockRunner.GetInputs();
            await base.OnInitializedAsync();
        }

        public void SelectScript()
        {
            Select?.Invoke(ScriptBlockRunner);
        }

        public async Task RunScript()
        {
            if (Inputs.Any())
            {
                await Task.Run(() =>
                {
                    RequestInputs(Inputs, async inputs =>
                    {
                        await ScriptBlockRunner.Run(Inputs);
                    });
                });
            }
            else
            {
                await ScriptBlockRunner.Run(Inputs);
            }
        }
    }
}
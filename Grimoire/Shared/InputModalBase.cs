using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Grimoire.Shared
{
    public class InputModalBase : ComponentBase
    {
        [Parameter]
        public Action<IList<Input>> ConfirmAction { get; set; }

        [Parameter]
        public IList<Input> Inputs { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        public void InputKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                ConfirmAction(Inputs);
            }
        }
    }
}
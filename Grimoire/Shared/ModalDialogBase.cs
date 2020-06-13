using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Grimoire.Shared
{
    public class ModalDialogBase : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public string ErrorDetail { get; set; }

        [Parameter]
        public Action<string, Action> LoginAction { get; set; }

        [Parameter]
        public Action OkAction { get; set; }

        [Parameter]
        public Action CancelAction { get; set; }

        [Parameter]
        public ModalType Type { get; set; }

        public bool LoginError { get; set; } = false;

        public string PinInput { get; set; } = "";

        public bool ConfirmResetVault { get; set; } = false;

        public void InvalidPin() => LoginError = true;

        public void InputKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                LoginAction(PinInput, InvalidPin);
            }
        }
    }
}
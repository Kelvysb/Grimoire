using System;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class ScriptBlockDetailBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness grimoireBusiness { get; set; }

        public IGrimoireRunner ScriptBlockRunner { get; set; }

        public bool Executing { get; set; }

        public bool DeleteModal { get; set; }

        public delegate void EditEventHandler(IGrimoireRunner scriptRunner);

        public event EditEventHandler Edit;

        public delegate void DeleteEventHandler(IGrimoireRunner scriptRunner);

        public event DeleteEventHandler Delete;

        public delegate void ErrorEventHandler(string message, Exception ex);

        public event ErrorEventHandler Error;

        protected override async Task OnInitializedAsync()
        {
            Executing = false;
            CloseModals();
            await base.OnInitializedAsync();
        }

        public Task EditScript()
        {
            return Task.Run(() => Edit?.Invoke(ScriptBlockRunner));
        }

        public async Task RunScript()
        {
            try
            {
                Executing = true;
                ScriptBlockRunner.ScriptBlock.LastResult = await ScriptBlockRunner.Run();
            }
            catch (Exception ex)
            {
                await Task.Run(() => Error?.Invoke("Error running script.", ex));
            }
            finally
            {
                await Task.Run(() => Executing = false);
                await InvokeAsync(() => StateHasChanged());
            }
        }

        public void ConfirmRemove()
        {
            DeleteModal = true;
        }

        public async Task RemoveScript()
        {
            try
            {
                await grimoireBusiness.RemoveScriptBlock(ScriptBlockRunner.ScriptBlock.Name);
                await Task.Run(() => Delete?.Invoke(ScriptBlockRunner));
            }
            catch (Exception ex)
            {
                await Task.Run(() => Error?.Invoke("Error removing script.", ex));
            }
        }

        public void Reload()
        {
            StateHasChanged();
        }

        public void CloseModals()
        {
            DeleteModal = false;
        }
    }
}
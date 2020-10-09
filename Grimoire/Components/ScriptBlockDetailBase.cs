using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class ScriptBlockDetailBase : ComponentBase
    {
        [Inject]
        public IGrimoireBusiness grimoireBusiness { get; set; }

        [Parameter]
        public Action<IList<Input>, Action<IList<Input>>> RequestInputs { get; set; }

        public IGrimoireRunner ScriptBlockRunner { get; set; }

        public bool Executing { get; set; }

        public bool DeleteModal { get; set; }

        public delegate void EditEventHandler(IGrimoireRunner scriptRunner);

        public event EditEventHandler Edit;

        public delegate Task DeleteEventHandler(IGrimoireRunner scriptRunner);

        public event DeleteEventHandler Delete;

        public delegate Task ErrorEventHandler(string message, Exception ex);

        public event ErrorEventHandler Error;

        private IList<Input> Inputs { get; set; }

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

                Inputs = await ScriptBlockRunner?.GetInputs();

                if (Inputs.Any())
                {
                    await ExecuteWithInputs();
                }
                else
                {
                    await Execute();
                }
            }
            catch (Exception ex)
            {
                await Task.Run(() => Error?.Invoke("Error running script.", ex));
            }
        }

        private async Task Execute()
        {
            try
            {
                ScriptBlockRunner.ScriptBlock.LastResult = await ScriptBlockRunner.Run(Inputs);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await Task.Run(() => Executing = false);
                await InvokeAsync(() => StateHasChanged());
            }
        }

        private async Task ExecuteWithInputs()
        {
            await Task.Run(() =>
            {
                RequestInputs(Inputs, async inputs =>
                {
                    try
                    {
                        ScriptBlockRunner.ScriptBlock.LastResult = await ScriptBlockRunner.Run(Inputs);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        await Task.Run(() => Executing = false);
                        await InvokeAsync(() => StateHasChanged());
                    }
                });
            });
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
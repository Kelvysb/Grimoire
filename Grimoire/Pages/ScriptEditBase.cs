using System;
using System.Threading.Tasks;
using BlazorInputFile;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Pages
{
    public class ScriptEditBase : ComponentBase
    {
        [Inject]
        private IGrimoireBusiness grimoireBusiness { get; set; }

        [Parameter]
        public string ScriptName { get; set; }

        public bool IsEdit { get; set; }

        public string ScriptTypeHolder { get; set; }

        public string AlertLevelHolder { get; set; }

        public string ExecutionModeHolder { get; set; }

        public GrimoireScriptBlock ScriptBlock { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await InitializeScriptBlock();
            await base.OnInitializedAsync();
        }

        public void HandleFileSelected(IFileListEntry[] files)
        {
            ScriptBlock.Script = files[0].Name;
            ScriptBlock.OriginalScriptFile = files[0].Data;
        }

        public async Task Submit()
        {
            CompleteValues();
            await grimoireBusiness.SaveScriptBlock(ScriptBlock);
        }

        private async Task InitializeScriptBlock()
        {
            if (!string.IsNullOrEmpty(ScriptName))
            {
                ScriptBlock = await grimoireBusiness.GetScriptBlock(ScriptName);
                IsEdit = true;
            }
            else
            {
                ScriptBlock = new GrimoireScriptBlock()
                {
                    Name = "",
                    Description = "",
                    AlertLevel = AlertLevel.Error,
                    ExtractResult = new PatternRange() { Start = "", End = "" },
                    Group = "",
                    Interval = 0,
                    Order = 0,
                    OriginalScriptFile = null,
                    Script = "",
                    ScriptBlocks = null,
                    ScriptType = ScriptType.PowerShell,
                    SuccessPatern = "",
                    TimeOut = 10,
                    ExecutionMode = ExecutionMode.Manual
                };
                IsEdit = false;
            }
            ScriptTypeHolder = ScriptBlock.ScriptType.ToString();
            AlertLevelHolder = ScriptBlock.AlertLevel.ToString();
            switch (ScriptBlock.ExecutionMode)
            {
                case ExecutionMode.RunOnStart:
                    ExecutionModeHolder = "Run on start";
                    break;

                case ExecutionMode.Interval:
                    ExecutionModeHolder = "Interval";
                    break;

                default:
                    ExecutionModeHolder = "Manual";
                    break;
            }
        }

        private void CompleteValues()
        {
            ScriptBlock.ScriptType = (ScriptType)Enum.Parse(typeof(ScriptType), ScriptTypeHolder);
            ScriptBlock.AlertLevel = (AlertLevel)Enum.Parse(typeof(AlertLevel), AlertLevelHolder);
            switch (ExecutionModeHolder)
            {
                case "Run on start":
                    ScriptBlock.ExecutionMode = ExecutionMode.RunOnStart;
                    break;

                case "Interval":
                    ScriptBlock.ExecutionMode = ExecutionMode.Interval;
                    break;

                default:
                    ScriptBlock.ExecutionMode = ExecutionMode.Manual;
                    break;
            }
        }
    }
}
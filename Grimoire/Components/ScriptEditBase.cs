using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlazorInputFile;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace Grimoire.Components
{
    public class ScriptEditBase : ComponentBase
    {
        [Inject]
        private IGrimoireBusiness Business { get; set; }

        public string ScriptName { get; set; }

        public bool IsEdit { get; set; }

        public string ScriptTypeHolder { get; set; }

        public string AlertLevelHolder { get; set; }

        public string ExecutionModeHolder { get; set; }

        public bool ConfirmModal { get; set; }

        public GrimoireScriptBlock ScriptBlock { get; set; }

        public List<string> ExistingGroups { get; set; } = new List<string>();

        public string SelectedScript { get; set; } = "";

        public string SelectedAdditionals { get; set; } = "";

        public string SelectedGroup
        {
            get { return ScriptBlock.Group; }
            set { ScriptBlock.Group = value; }
        }

        public delegate Task EndEditHandler();

        public event EndEditHandler EndEdit;

        public delegate Task ErrorEventHandler(string message, Exception ex);

        public event ErrorEventHandler Error;

        protected async override Task OnInitializedAsync()
        {
            ConfirmModal = false;
            await InitializeScriptBlock();
            LoadGroups();
            await base.OnInitializedAsync();
        }

        public async void HandleFileSelected(IFileListEntry[] files)
        {
            if (files.Length > 0)
            {
                SelectedScript = files[0].Name;
                ScriptBlock.Script = files[0].Name;
                MemoryStream stream = new MemoryStream();
                await files[0].Data.CopyToAsync(stream);
                ScriptBlock.OriginalScriptFile = stream;
            }
            else
            {
                SelectedScript = "";
                ScriptBlock.Script = "";
                ScriptBlock.OriginalScriptFile = null;
            }
        }

        public async void HandleAdditionalFilesSelected(IFileListEntry[] files)
        {
            if (files.Length > 0)
            {
                SelectedAdditionals = string.Join(", ", files.Select(file => file.Name));
                ScriptBlock.AdditionalFiles = new List<AdditionalFile>();
                foreach (var file in files)
                {
                    MemoryStream stream = new MemoryStream();
                    await file.Data.CopyToAsync(stream);
                    ScriptBlock.AdditionalFiles.Add(new AdditionalFile(file.Name, stream));
                }
            }
            else
            {
                SelectedAdditionals = "";
                ScriptBlock.AdditionalFiles = new List<AdditionalFile>();
            }
        }

        public async void Reload()
        {
            await InitializeScriptBlock();
            LoadGroups();
            StateHasChanged();
        }

        public async Task Submit()
        {
            try
            {
                CompleteValues();
                await Business.SaveScriptBlock(ScriptBlock);
                EndEdit?.Invoke();
                ConfirmModal = true;
            }
            catch (Exception ex)
            {
                await Task.Run(() => Error?.Invoke("Error saving the script.", ex));
            }
        }

        public void CloseModals()
        {
            ConfirmModal = false;
        }

        private void LoadGroups()
        {
            if (Business.ScriptRunners.Any())
            {
                ExistingGroups = Business.ScriptRunners
                    .Select(runner => runner.ScriptBlock.Group)
                    .Distinct()
                    .OrderBy(group => group)
                    .ToList();
            }
        }

        private async Task InitializeScriptBlock()
        {
            try
            {
                if (!string.IsNullOrEmpty(ScriptName))
                {
                    ScriptBlock = await Business.GetScriptBlock(ScriptName);
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
                        Interval = 30,
                        Order = 0,
                        OriginalScriptFile = null,
                        AdditionalFiles = new List<AdditionalFile>(),
                        Script = "",
                        ScriptBlocks = null,
                        ScriptType = ScriptType.PowerShell,
                        SuccessPatern = "",
                        TimeOut = 10,
                        ExecutionMode = ExecutionMode.Manual
                    };
                    IsEdit = false;
                }
                if (ScriptBlock != null)
                {
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
            }
            catch (Exception ex)
            {
                await Task.Run(() => Error?.Invoke("Error loading the script.", ex));
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
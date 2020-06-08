using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Services
{
    public class GrimoireService : ResourceLoaderBase, IGrimoireService
    {
        private readonly IConfigurationService configurationService;

        public GrimoireService(IConfigurationService configurationService,
                               ILogService logService)
        {
            this.configurationService = configurationService;
            this.logService = logService;
        }

        public Task<GrimoireConfig> GetConfig()
        {
            return Task.Run(() => configurationService.Config);
        }

        public IEnumerable<ExecutionGroup> GetExecutionGroups()
        {
            return GetResources<ExecutionGroup>(configurationService.ExecutionGroupsDirectory);
        }

        public GrimoireScriptBlock GetScriptBlock(string name)
        {
            return GetResource<GrimoireScriptBlock>(configurationService.ScriptsDirectory, name);
        }

        public IEnumerable<GrimoireScriptBlock> GetScriptBlocks()
        {
            return GetResources<GrimoireScriptBlock>(configurationService.ScriptsDirectory)
                .ToList();
        }

        public string getScriptFullPath(GrimoireScriptBlock scriptBlock)
        {
            return Path.GetDirectoryName(
                    Path.GetFullPath(
                    Path.Combine(configurationService.ScriptsDirectory,
                                    scriptBlock.Name,
                                    scriptBlock.Script)));
        }

        public void RemoveExecutionGroup(string name)
        {
            string resourcePath = Path.Combine(configurationService.ExecutionGroupsDirectory, $"{name}.json");
            RemoveResource(resourcePath);
        }

        public void RemoveScriptBlock(string name)
        {
            string resourcePath = Path.Combine(configurationService.ScriptsDirectory, $"{name}.json");
            RemoveResource(resourcePath);
        }

        public Task SaveConfig(GrimoireConfig config)
        {
            configurationService.Config.BashPath = config.BashPath;
            configurationService.Config.DefaultScriptEditor = config.DefaultScriptEditor;
            configurationService.Config.Theme = config.Theme;
            return Task.Run(() => configurationService.SaveConfig());
        }

        public void SaveExecutionGroup(ExecutionGroup executionGroup)
        {
            executionGroup.Name = CleanResourceName(executionGroup.Name);
            string resourcePath = Path.Combine(configurationService.ExecutionGroupsDirectory, $"{executionGroup.Name}.json");
            EnsurePath(configurationService.ExecutionGroupsDirectory);
            SaveResource(executionGroup, resourcePath);
        }

        public void SaveScriptBlocks(GrimoireScriptBlock scriptBlock)
        {
            scriptBlock.Name = CleanResourceName(scriptBlock.Name);
            string resourcePath = Path.Combine(configurationService.ScriptsDirectory, $"{scriptBlock.Name}.json");
            string resourceFolderPath = Path.Combine(configurationService.ScriptsDirectory, scriptBlock.Name);
            string scriptPath = Path.Combine(resourceFolderPath, scriptBlock.Script);
            EnsurePath(resourceFolderPath);
            SaveResource(scriptBlock, resourcePath);
            SaveScriptFile(scriptBlock, scriptPath);
            SaveAdditionalFiles(scriptBlock, resourceFolderPath);
        }

        private void SaveAdditionalFiles(GrimoireScriptBlock scriptBlock, string resourceFolderPath)
        {
            if (scriptBlock.AdditionalFiles != null && scriptBlock.AdditionalFiles.Any())
            {
                foreach (var additionalFile in scriptBlock.AdditionalFiles)
                {
                    string additionalFilePath = Path.Combine(resourceFolderPath, additionalFile.Name);
                    using (FileStream file = File.Create(additionalFilePath))
                    {
                        additionalFile.File.Seek(0, SeekOrigin.Begin);
                        additionalFile.File.CopyTo(file);
                        file.Close();
                    }
                }
            }
        }

        private void SaveScriptFile(GrimoireScriptBlock scriptBlock, string scriptPath)
        {
            if (scriptBlock.OriginalScriptFile != null)
            {
                using (FileStream file = File.Create(scriptPath))
                {
                    scriptBlock.OriginalScriptFile.Seek(0, SeekOrigin.Begin);
                    scriptBlock.OriginalScriptFile.CopyTo(file);
                    file.Close();
                }
            }
        }
    }
}
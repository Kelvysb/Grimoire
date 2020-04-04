using System.Collections.Generic;
using System.IO;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Services
{
    public class GrimoireService : ResourceLoaderBase, IGrimoireService
    {
        private readonly IConfigurationService configurationService;

        public GrimoireService(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public ICollection<ExecutionGroup> GetExecutionGroups()
        {
            return GetResources<ExecutionGroup>(configurationService.ExecutionGroupsDirectory);
        }

        public GrimoireScriptBlock GetScriptBlock(string name)
        {
            return GetResource<GrimoireScriptBlock>(configurationService.ExecutionGroupsDirectory, name);
        }

        public ICollection<GrimoireScriptBlock> GetScriptBlocks()
        {
            return GetResources<GrimoireScriptBlock>(configurationService.ScriptsDirectory);
        }

        public string getScriptFullPath(GrimoireScriptBlock scriptBlock)
        {
            return Path.GetFullPath(
                Path.Combine(configurationService.ScriptsDirectory,
                             scriptBlock.Name,
                             scriptBlock.Script));
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
            string scriptPath = Path.Combine(resourceFolderPath, Path.GetFileName(scriptBlock.OriginalScriptPath));
            scriptBlock.Script = Path.GetFileName(scriptBlock.OriginalScriptPath);
            if (File.Exists(resourcePath))
            {
                RemoveResource(resourcePath);
            }
            EnsurePath(resourceFolderPath);
            SaveResource(scriptBlock, resourcePath);
            File.Copy(scriptBlock.OriginalScriptPath, scriptPath);
        }
    }
}
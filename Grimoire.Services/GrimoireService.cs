using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return GetResource<GrimoireScriptBlock>(configurationService.ScriptsDirectory, name);
        }

        public ICollection<GrimoireScriptBlock> GetScriptBlocks()
        {
            return GetResources<GrimoireScriptBlock>(configurationService.ScriptsDirectory)
                .Select(s =>
                {
                    string path = Path.Combine(configurationService.ScriptsDirectory, s.Name, s.Script);
                    s.OriginalScriptFile = LoadScriptStream(path);
                    return s;
                })
                .ToList();
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
            string scriptPath = Path.Combine(resourceFolderPath, scriptBlock.Script);
            if (File.Exists(resourcePath))
            {
                RemoveResource(resourcePath);
            }
            EnsurePath(resourceFolderPath);
            SaveResource(scriptBlock, resourcePath);
            using (StreamWriter file = new StreamWriter(scriptPath, false))
            {
                file.Write(scriptBlock.OriginalScriptFile);
                file.Close();
            }
        }
    }
}
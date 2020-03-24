using System;
using System.IO;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Services
{
    public class ConfigurationService : ResourceLoaderBase, IConfigurationService
    {
        private const string AppName = "grimoire";
        private const string ConfigFileName = "grimoire.cfg";

        public ConfigurationService()
        {
            LoadWorkDirectory();
            LoadConfig();
        }

        public GrimoireConfig Config { get; private set; }

        public string WorkDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), AppName);

        public string ExecutionGroupsDirectory => Path.Combine(WorkDirectory, "executionGroups");

        public string ScriptsDirectory => Path.Combine(WorkDirectory, "scripts");

        private string ConfigurationFile => Path.Combine(WorkDirectory, ConfigFileName);

        public void SaveConfig()
        {
            SaveResource(Config, ConfigurationFile);
        }

        private void LoadConfig()
        {
            Config = GetResourceFile<GrimoireConfig>(ConfigurationFile);
            if (Config == null)
            {
                CreateDefaultConfig();
                SaveConfig();
            }
        }

        private void CreateDefaultConfig()
        {
            Config = new GrimoireConfig()
            {
                DefaultScriptEditor = "notepad.exe",
                BashPath = ""
            };
        }

        private void LoadWorkDirectory()
        {
            EnsurePath(WorkDirectory);
        }
    }
}
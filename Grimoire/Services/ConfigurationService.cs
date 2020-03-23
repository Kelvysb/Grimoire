using System;
using System.IO;
using System.Text.Json;
using Grimoire.Models;

namespace Grimoire.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private const string AppName = "Grimoire";
        private const string ConfigFileName = "Grimoire.cfg";

        public ConfigurationService()
        {
            LoadWorkDirectory();
            LoadConfig();
        }

        public GrimoireConfig Config { get; private set; }

        public string WorkDirectory { get; private set; }

        public void SaveConfig()
        {
            StreamWriter file = new StreamWriter(Path.Combine(WorkDirectory, ConfigFileName), false);
            file.Write(JsonSerializer.Serialize(Config));
            file.Close();
        }

        private void LoadConfig()
        {
            if (File.Exists(Path.Combine(WorkDirectory, ConfigFileName)))
            {
                try
                {
                    StreamReader file = new StreamReader(Path.Combine(WorkDirectory, ConfigFileName));
                    string fileContent = file.ReadToEnd();
                    file.Close();
                    Config = JsonSerializer.Deserialize<GrimoireConfig>(fileContent);
                }
                catch (Exception)
                {
                    File.Delete(Path.Combine(WorkDirectory, ConfigFileName));
                    LoadConfig();
                }
            }
            else
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
            WorkDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), AppName);
            if (!Directory.Exists(WorkDirectory))
            {
                Directory.CreateDirectory(WorkDirectory);
            }
        }
    }
}
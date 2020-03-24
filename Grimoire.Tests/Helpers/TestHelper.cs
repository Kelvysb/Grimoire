using System.IO;
using System.Text.Json;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Tests.Helpers
{
    internal static class TestHelper
    {
        public static string TestFile => Path.Combine(".", "TestData", "TEstScript.ps1");

        public static ScriptBlock GetSingleTestScript()
        {
            return new ScriptBlock()
            {
                Order = 0,
                Group = "Test",
                Name = "Test Script",
                Description = "Test Script",
                AlertLevel = AlertLevel.Warning,
                Script = "TestScript.ps1",
                OriginalScriptPath = TestFile,
                Interval = 0,
                ScriptType = ScriptType.PowerShell,
                SuccessPatern = "Count done"
            };
        }

        public static void CleanTestFolders(IConfigurationService config)
        {
            if (Directory.Exists(config.WorkDirectory))
            {
                Directory.Delete(config.WorkDirectory, true);
            }
            Directory.CreateDirectory(config.WorkDirectory);
        }

        public static ScriptBlock GetCreatedScript(IConfigurationService config, string scriptFileName)
        {
            ScriptBlock result = null;
            if (File.Exists(Path.Combine(config.ScriptsDirectory, scriptFileName)))
            {
                using (StreamReader reader = new StreamReader(Path.Combine(config.ScriptsDirectory, scriptFileName)))
                {
                    string file = reader.ReadToEnd();
                    reader.Close();
                    result = JsonSerializer.Deserialize<ScriptBlock>(file);
                }
            }
            return result;
        }

        public static bool CheckCreatedScript(IConfigurationService config, string name, string script)
        {
            return File.Exists(Path.Combine(config.ScriptsDirectory, name, script));
        }
    }
}
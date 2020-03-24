using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Tests.Helpers
{
    internal static class TestHelper
    {
        public static string TestFile => Path.Combine(".", "TestData", "TestScript.ps1");

        public static ExecutionGroup GetSingleExecutionGroup()
        {
            return new ExecutionGroup()
            {
                Order = 0,
                Name = "Test Script",
                Description = "Test Script",
                Scripts = new List<string>()
                {
                    "Test_Script_1",
                    "Test_Script_2",
                    "Test_Script_3",
                    "Test_Script_4",
                    "Test_Script_5"
                }
            };
        }

        public static List<ExecutionGroup> GetMultipleExecutionGroup()
        {
            return new List<ExecutionGroup>()
            {
                new ExecutionGroup()
                {
                    Order = 0,
                    Name = "Test Group 1",
                    Description = "Test Group 1",
                    Scripts = new List<string>()
                    {
                        "Test_Script_1",
                        "Test_Script_2",
                        "Test_Script_3",
                        "Test_Script_4",
                        "Test_Script_5"
                    }
                },
                new ExecutionGroup()
                {
                    Order = 0,
                    Name = "Test Group 2",
                    Description = "Test Group 2",
                    Scripts = new List<string>()
                    {
                        "Test_Script_1",
                        "Test_Script_6"
                    }
                }
            };
        }

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

        public static List<ScriptBlock> GetMultipleTestScript()
        {
            return new List<ScriptBlock>()
            {
                new ScriptBlock()
                {
                    Order = 0,
                    Group = "Test",
                    Name = "Test Script 1",
                    Description = "Test Script 1",
                    AlertLevel = AlertLevel.Warning,
                    Script = "TestScript.ps1",
                    OriginalScriptPath = TestFile,
                    Interval = 0,
                    ScriptType = ScriptType.PowerShell,
                    SuccessPatern = "Count done"
                },
                new ScriptBlock()
                {
                    Order = 1,
                    Group = "Test",
                    Name = "Test Script 2",
                    Description = "Test Script 2",
                    AlertLevel = AlertLevel.Warning,
                    Script = "TestScript.ps1",
                    OriginalScriptPath = TestFile,
                    Interval = 0,
                    ScriptType = ScriptType.PowerShell,
                    SuccessPatern = "Count done"
                },
                new ScriptBlock()
                {
                    Order = 2,
                    Group = "Group",
                    Name = "Test Script 3",
                    Description = "Test Script 3",
                    AlertLevel = AlertLevel.Warning,
                    Script = "TestScript.ps1",
                    OriginalScriptPath = TestFile,
                    Interval = 0,
                    ScriptType = ScriptType.PowerShell,
                    SuccessPatern = "Count done"
                },
                new ScriptBlock()
                {
                    Order = 3,
                    Group = "Group",
                    Name = "Test Script 4",
                    Description = "Test Script 4",
                    AlertLevel = AlertLevel.Warning,
                    Script = "TestScript.ps1",
                    OriginalScriptPath = TestFile,
                    Interval = 0,
                    ScriptType = ScriptType.PowerShell,
                    SuccessPatern = "Count done"
                },
                new ScriptBlock()
                {
                    Order = 4,
                    Group = "Group",
                    Name = "Test Script 5",
                    Description = "Test Script 5",
                    AlertLevel = AlertLevel.Warning,
                    Script = "TestScript.ps1",
                    OriginalScriptPath = TestFile,
                    Interval = 0,
                    ScriptType = ScriptType.PowerShell,
                    SuccessPatern = "Count done"
                },
                new ScriptBlock()
                {
                    Order = 5,
                    Group = "Group",
                    Name = "Test Script 6",
                    Description = "Test Script 6",
                    AlertLevel = AlertLevel.Warning,
                    Script = "TestScript.ps1",
                    OriginalScriptPath = TestFile,
                    Interval = 0,
                    ScriptType = ScriptType.PowerShell,
                    SuccessPatern = "Count done"
                }
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

        public static ExecutionGroup GetCreatedExecutionGroup(IConfigurationService config, string groupName)
        {
            ExecutionGroup result = null;
            if (File.Exists(Path.Combine(config.ExecutionGroupsDirectory, groupName)))
            {
                using (StreamReader reader = new StreamReader(Path.Combine(config.ExecutionGroupsDirectory, groupName)))
                {
                    string file = reader.ReadToEnd();
                    reader.Close();
                    result = JsonSerializer.Deserialize<ExecutionGroup>(file);
                }
            }
            return result;
        }
    }
}
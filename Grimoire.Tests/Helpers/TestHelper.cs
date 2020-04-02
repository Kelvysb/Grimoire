using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Grimoire.Tests.Helpers
{
    internal static class TestHelper
    {
        public static string TestFile => Path.Combine(".", "TestData", "TestScript.ps1");

        public static string TestFileWarning => Path.Combine(".", "TestData", "TestScriptWarning.ps1");

        public static string TestFileError => Path.Combine(".", "TestData", "TestScriptError.ps1");

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

        public static GrimoireScriptBlock GetSingleTestScript()
        {
            return new GrimoireScriptBlock()
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
                SuccessPatern = "Count done",
                ExtractResult = new PatternRange()
                {
                    Start = "Result group",
                    End = "End"
                },
                TimeOut = 10
            };
        }

        public static GrimoireScriptBlock GetDefaultWarningTestScript()
        {
            return new GrimoireScriptBlock()
            {
                Order = 0,
                Group = "Test",
                Name = "Test Script Warning",
                Description = "Test Script Warning",
                AlertLevel = AlertLevel.Warning,
                Script = "TestScriptWarning.ps1",
                OriginalScriptPath = TestFileWarning,
                Interval = 0,
                ScriptType = ScriptType.PowerShell,
                SuccessPatern = "",
                ExtractResult = null,
                TimeOut = 10
            };
        }

        public static GrimoireScriptBlock GetDefaultErrorTestScript()
        {
            return new GrimoireScriptBlock()
            {
                Order = 0,
                Group = "Test",
                Name = "Test Script Error",
                Description = "Test Script Error",
                AlertLevel = AlertLevel.Error,
                Script = "TestScriptError.ps1",
                OriginalScriptPath = TestFileError,
                Interval = 0,
                ScriptType = ScriptType.PowerShell,
                SuccessPatern = "",
                ExtractResult = null,
                TimeOut = 10
            };
        }

        public static List<GrimoireScriptBlock> GetMultipleTestScript()
        {
            return new List<GrimoireScriptBlock>()
            {
                new GrimoireScriptBlock()
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
                    SuccessPatern = "Count done",
                    ExtractResult = new PatternRange()
                    {
                        Start = "Result group",
                        End = "End"
                    },
                    TimeOut = 10
                },
                new GrimoireScriptBlock()
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
                    SuccessPatern = "Count done",
                    ExtractResult = new PatternRange()
                    {
                        Start = "Result group",
                        End = "End"
                    },
                    TimeOut = 10
                },
                new GrimoireScriptBlock()
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
                    SuccessPatern = "Count done",
                    ExtractResult = new PatternRange()
                    {
                        Start = "Result group",
                        End = "End"
                    },
                    TimeOut = 10
                },
                new GrimoireScriptBlock()
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
                    SuccessPatern = "Count done",
                    ExtractResult = new PatternRange()
                    {
                        Start = "Result group",
                        End = "End"
                    },
                    TimeOut = 10
                },
                new GrimoireScriptBlock()
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
                    SuccessPatern = "Count done",
                    ExtractResult = new PatternRange()
                    {
                        Start = "Result group",
                        End = "End"
                    },
                    TimeOut = 10
                },
                new GrimoireScriptBlock()
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
                    SuccessPatern = "Count done",
                    ExtractResult = new PatternRange()
                    {
                        Start = "Result group",
                        End = "End"
                    },
                    TimeOut = 10
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

        public static GrimoireScriptBlock GetCreatedScript(IConfigurationService config, string scriptFileName)
        {
            GrimoireScriptBlock result = null;
            if (File.Exists(Path.Combine(config.ScriptsDirectory, scriptFileName)))
            {
                using (StreamReader reader = new StreamReader(Path.Combine(config.ScriptsDirectory, scriptFileName)))
                {
                    string file = reader.ReadToEnd();
                    reader.Close();
                    result = JsonSerializer.Deserialize<GrimoireScriptBlock>(file);
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
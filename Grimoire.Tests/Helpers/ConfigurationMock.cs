using System.IO;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;
using Moq;

namespace Grimoire.Tests.Helpers
{
    internal static class ConfigurationMock
    {
        public static IConfigurationService GetConfiguration()
        {
            Mock<IConfigurationService> result = new Mock<IConfigurationService>();
            result.Setup(config => config.Config).Returns(new GrimoireConfig() { BashPath = "", DefaultScriptEditor = "Notepad.exe" });
            result.Setup(config => config.WorkDirectory).Returns(Path.Combine(".", "grimoireTest"));
            result.Setup(config => config.ScriptsDirectory).Returns(Path.Combine(".", "grimoireTest", "scripts"));
            result.Setup(config => config.ExecutionGroupsDirectory).Returns(Path.Combine(".", "grimoireTest", "executionGroups"));
            return result.Object;
        }
    }
}
using System.IO;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;
using Moq;

namespace Grimoire.Tests.Helpers
{
    internal static class ServiceMock
    {
        public static IConfigurationService GetConfiguration()
        {
            Mock<IConfigurationService> result = new Mock<IConfigurationService>();
            result.Setup(config => config.Config).Returns(new GrimoireConfig()
            {
                BashPath = "",
                DefaultScriptEditor = "Notepad.exe",
                ShowGroup = true,
                Theme = "Dark"
            });
            result.Setup(config => config.WorkDirectory).Returns(Path.Combine(".", "grimoireTest"));
            result.Setup(config => config.ScriptsDirectory).Returns(Path.Combine(".", "grimoireTest", "scripts"));
            result.Setup(config => config.ExecutionGroupsDirectory).Returns(Path.Combine(".", "grimoireTest", "executionGroups"));
            return result.Object;
        }

        public static IVaultService GetVaultService()
        {
            Mock<IVaultService> result = new Mock<IVaultService>();
            result.Setup(vault => vault.CheckPin(It.IsAny<string>())).Returns(Task.Run(() => true));
            result.Setup(vault => vault.GetVault(It.IsAny<string>())).Returns(Task.Run(() => new Vault()));
            result.Setup(vault => vault.SaveVault(It.IsAny<Vault>(), It.IsAny<string>()));
            result.Setup(vault => vault.ResetVault());
            return result.Object;
        }
    }
}
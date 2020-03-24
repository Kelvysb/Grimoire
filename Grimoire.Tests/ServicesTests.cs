using FluentAssertions;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;
using Grimoire.Services;
using Grimoire.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimoire.Tests
{
    [TestClass]
    public class ServicesTests
    {
        private readonly IConfigurationService config;
        private readonly IGrimoireService service;

        public ServicesTests()
        {
            config = ConfigurationMock.GetConfiguration();
            TestHelper.CleanTestFolders(config);
            service = new GrimoireService(config);
        }

        [TestMethod]
        public void SaveScriptTest()
        {
            ScriptBlock script = TestHelper.GetSingleTestScript();
            service.SaveScriptBlocks(script);
            ScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            TestHelper.CheckCreatedScript(config, result.Name, result.Script).Should().BeTrue();
        }
    }
}
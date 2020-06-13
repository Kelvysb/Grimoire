using System.Collections.Generic;
using System.Linq;
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
            service = new GrimoireService(config, new LogService(config));
        }

        [TestMethod]
        public void SaveScriptTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            service.SaveScriptBlocks(script);
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            TestHelper.CheckCreatedScript(config, result.Name, result.Script).Should().BeTrue();
        }

        [TestMethod]
        public void GetScriptListTest()
        {
            AddMultipleScript();
            List<GrimoireScriptBlock> result = service.GetScriptBlocks().ToList();
            result.Should().NotBeNull();
            result.Any().Should().BeTrue();
            result.Count.Should().BeGreaterOrEqualTo(6);
        }

        [TestMethod]
        public void RemoveScriptTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            service.SaveScriptBlocks(script);
            GrimoireScriptBlock inserted = TestHelper.GetCreatedScript(config, "Test_Script.json");
            inserted.Should().NotBeNull();
            service.RemoveScriptBlock(inserted.Name);
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().BeNull();
        }

        [TestMethod]
        public void SaveExecutionGroupTest()
        {
            ExecutionGroup group = TestHelper.GetSingleExecutionGroup();
            service.SaveExecutionGroup(group);
            ExecutionGroup result = TestHelper.GetCreatedExecutionGroup(config, "Test_Script.json");
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetExecutionGroupListTest()
        {
            AddMultipleExecutionGroup();
            List<ExecutionGroup> result = service.GetExecutionGroups().ToList();
            result.Should().NotBeNull();
            result.Any().Should().BeTrue();
            result.Count.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        public void RemoveExecutionGroupTest()
        {
            ExecutionGroup group = TestHelper.GetSingleExecutionGroup();
            service.SaveExecutionGroup(group);
            ExecutionGroup inserted = TestHelper.GetCreatedExecutionGroup(config, "Test_Script.json");
            inserted.Should().NotBeNull();
            service.RemoveExecutionGroup(inserted.Name);
            ExecutionGroup result = TestHelper.GetCreatedExecutionGroup(config, "Test_Script.json");
            result.Should().BeNull();
        }

        private void AddMultipleScript()
        {
            foreach (GrimoireScriptBlock script in TestHelper.GetMultipleTestScript())
            {
                service.SaveScriptBlocks(script);
            }
        }

        private void AddMultipleExecutionGroup()
        {
            foreach (ExecutionGroup group in TestHelper.GetMultipleExecutionGroup())
            {
                service.SaveExecutionGroup(group);
            }
        }
    }
}
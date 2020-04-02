using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Grimoire.Business;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;
using Grimoire.Services;
using Grimoire.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grimoire.Tests
{
    [TestClass]
    public class BusinessTests
    {
        private readonly IConfigurationService config;
        private readonly IGrimoireBusiness business;

        public BusinessTests()
        {
            config = ConfigurationMock.GetConfiguration();
            TestHelper.CleanTestFolders(config);
            business = new GrimoireBusiness(new GrimoireService(config));
        }

        [TestMethod]
        public void ScriptExecutionSuccessTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            ScriptResult scriptResult = business.ExecuteScript(result).Result;
            scriptResult.Should().NotBeNull();
            scriptResult.ResultType.Should().Be(ResultType.Success);
            scriptResult.RawResult.Length.Should().Be(1869);
            scriptResult.FilteredResult.Should()
                .Contain("Result 1")
                .And
                .Contain("Result 2")
                .And
                .Contain("Result 3")
                .And
                .Contain("Result 4");
        }

        [TestMethod]
        public void ScriptExecutionSoftWarningTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            result.AlertLevel = AlertLevel.Warning;
            result.SuccessPatern = "Soft Warning";
            ScriptResult scriptResult = business.ExecuteScript(result).Result;
            scriptResult.Should().NotBeNull();
            scriptResult.ResultType.Should().Be(ResultType.Warning);
        }

        [TestMethod]
        public void ScriptExecutionSoftErrorTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            result.AlertLevel = AlertLevel.Error;
            result.SuccessPatern = "Soft Error";
            ScriptResult scriptResult = business.ExecuteScript(result).Result;
            scriptResult.Should().NotBeNull();
            scriptResult.ResultType.Should().Be(ResultType.Error);
        }

        [TestMethod]
        public void ScriptExecutionDefaultWarningTest()
        {
            GrimoireScriptBlock script = TestHelper.GetDefaultWarningTestScript();
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script_Warning.json");
            result.Should().NotBeNull();
            ScriptResult scriptResult = business.ExecuteScript(result).Result;
            scriptResult.Should().NotBeNull();
            scriptResult.ResultType.Should().Be(ResultType.Warning);
        }

        [TestMethod]
        public void ScriptExecutionDefaultErrorTest()
        {
            GrimoireScriptBlock script = TestHelper.GetDefaultErrorTestScript();
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script_Error.json");
            result.Should().NotBeNull();
            ScriptResult scriptResult = business.ExecuteScript(result).Result;
            scriptResult.Should().NotBeNull();
            scriptResult.ResultType.Should().Be(ResultType.Error);
        }

        [TestMethod]
        public void SaveScriptBlockTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetScriptListTest()
        {
            AddMultipleScript();
            List<GrimoireScriptBlock> result = business.GetScriptBlocks().Result.ToList();
            result.Should().NotBeNull();
            result.Any().Should().BeTrue();
            result.Count.Should().BeGreaterOrEqualTo(6);
        }

        [TestMethod]
        public void RemoveScriptTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock inserted = TestHelper.GetCreatedScript(config, "Test_Script.json");
            inserted.Should().NotBeNull();
            business.RemoveScriptBlock(inserted.Name).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().BeNull();
        }

        [TestMethod]
        public void SaveExecutionGroupTest()
        {
            ExecutionGroup group = TestHelper.GetSingleExecutionGroup();
            business.SaveExecutionGroup(group).Wait();
            ExecutionGroup result = TestHelper.GetCreatedExecutionGroup(config, "Test_Script.json");
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void GetExecutionGroupListTest()
        {
            AddMultipleExecutionGroup();
            List<ExecutionGroup> result = business.GetExecutionGroups().Result.ToList();
            result.Should().NotBeNull();
            result.Any().Should().BeTrue();
            result.Count.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        public void RemoveExecutionGroupTest()
        {
            ExecutionGroup group = TestHelper.GetSingleExecutionGroup();
            business.SaveExecutionGroup(group).Wait();
            ExecutionGroup inserted = TestHelper.GetCreatedExecutionGroup(config, "Test_Script.json");
            inserted.Should().NotBeNull();
            business.RemoveExecutionGroup(inserted.Name).Wait();
            ExecutionGroup result = TestHelper.GetCreatedExecutionGroup(config, "Test_Script.json");
            result.Should().BeNull();
        }

        private void AddMultipleScript()
        {
            foreach (GrimoireScriptBlock script in TestHelper.GetMultipleTestScript())
            {
                business.SaveScriptBlock(script).Wait();
            }
        }

        private void AddMultipleExecutionGroup()
        {
            foreach (ExecutionGroup group in TestHelper.GetMultipleExecutionGroup())
            {
                business.SaveExecutionGroup(group).Wait();
            }
        }
    }
}
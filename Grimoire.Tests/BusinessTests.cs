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
        private readonly IVaultService vault;
        private readonly IGrimoireBusiness business;
        private Vault savedVault;

        public BusinessTests()
        {
            savedVault = new Vault();
            config = ServiceMock.GetConfiguration();
            vault = ServiceMock.GetVaultService(savedVault);
            TestHelper.CleanTestFolders(config);
            business = new GrimoireBusiness(new GrimoireService(config, new LogService(config)), vault);
        }

        [TestMethod]
        public void ScriptExecutionSuccessTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            business.LoadScriptRunners().Wait();
            business.LoadVault().Wait();
            ScriptResult scriptResult = business.ScriptRunners.First().Run().Result;
            scriptResult.Should().NotBeNull();
            scriptResult.ResultType.Should().Be(ResultType.Success);
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
            script.AlertLevel = AlertLevel.Warning;
            script.SuccessPatern = "Soft Warning";
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            business.LoadScriptRunners().Wait();
            business.LoadVault().Wait();
            ScriptResult scriptResult = business.ScriptRunners.First().Run().Result;
            scriptResult.Should().NotBeNull();
            scriptResult.ResultType.Should().Be(ResultType.Warning);
        }

        [TestMethod]
        public void ScriptExecutionSoftErrorTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            script.AlertLevel = AlertLevel.Error;
            script.SuccessPatern = "Soft Error";
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            business.LoadScriptRunners().Wait();
            business.LoadVault().Wait();
            ScriptResult scriptResult = business.ScriptRunners.First().Run().Result;
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
            business.LoadScriptRunners().Wait();
            business.LoadVault().Wait();
            ScriptResult scriptResult = business.ScriptRunners.First().Run().Result;
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
            business.LoadScriptRunners().Wait();
            business.LoadVault().Wait();
            ScriptResult scriptResult = business.ScriptRunners.First().Run().Result;
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
            List<GrimoireScriptBlock> result = business.GetScriptBlocks().Result;
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
            List<ExecutionGroup> result = business.GetExecutionGroups().Result;
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

        [TestMethod]
        public void VerifyKeysTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            business.LoadVault();
            business.VerifyKeys(script).Wait();
            savedVault.Itens.Should().HaveCount(3);
            savedVault.Itens.Should()
                .Contain(item => item.Key.Equals("Key 1"))
                .And
                .Contain(item => item.Key.Equals("Key 2"))
                .And
                .Contain(item => item.Key.Equals("Key 3"));
            savedVault.Itens = new List<VaultItem>();
        }

        [TestMethod]
        public void VaultExecutionTest()
        {
            GrimoireScriptBlock script = TestHelper.GetSingleTestScript();
            script.ExtractResult = new PatternRange() { Start = "", End = "" };
            business.SaveScriptBlock(script).Wait();
            GrimoireScriptBlock result = TestHelper.GetCreatedScript(config, "Test_Script.json");
            result.Should().NotBeNull();
            savedVault.Itens = new List<VaultItem>();
            savedVault.Itens.Add(new VaultItem(true, "Key 1", "value 1"));
            savedVault.Itens.Add(new VaultItem(true, "Key 2", "value 2"));
            savedVault.Itens.Add(new VaultItem(true, "Key 3", "value 3"));
            business.LoadVault();
            business.LoadScriptRunners().Wait();
            ScriptResult scriptResult = business.ScriptRunners.First().Run().Result;
            scriptResult.Should().NotBeNull();
            scriptResult.ResultType.Should().Be(ResultType.Success);
            scriptResult.FilteredResult.Should()
                .Contain("Single line multiple keys: value 1 - value 2")
                .And
                .Contain("Single key: value 3");
            savedVault.Itens = new List<VaultItem>();
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
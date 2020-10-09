using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Business;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Business
{
    public class GrimoireBusiness : IGrimoireBusiness
    {
        private const string DEFAULT_PIN = "34ea6ffe0065892cd30a277c4570740e";

        private string pin = "";

        private readonly IGrimoireService grimoireService;

        private readonly IVaultService vaultService;

        public bool IsDefaultPin { get => pin.Equals(DEFAULT_PIN); }

        public IEnumerable<IGrimoireRunner> ScriptRunners { get; set; }

        public Vault Vault { get; set; } = null;

        public GrimoireConfig Config { get; set; } = new GrimoireConfig();

        public GrimoireBusiness(IGrimoireService grimoireService,
                                IVaultService vaultService)
        {
            this.grimoireService = grimoireService;
            this.vaultService = vaultService;
            CheckDefaultPin();
        }

        #region Script

        public async Task<List<GrimoireScriptBlock>> GetScriptBlocks()
        {
            return await Task.Run(() => grimoireService.GetScriptBlocks().ToList());
        }

        public async Task SaveScriptBlock(GrimoireScriptBlock scriptBlock)
        {
            await Task.Run(() => grimoireService.SaveScriptBlocks(scriptBlock));
        }

        public async Task RemoveScriptBlock(string name)
        {
            await Task.Run(() => grimoireService.RemoveScriptBlock(name));
        }

        public async Task ChangeScripBlockOrder(GrimoireScriptBlock scriptBlock, int orderShift)
        {
            await Task.Run(() => "");
        }

        public async Task<List<ExecutionGroup>> GetExecutionGroups()
        {
            return await Task.Run(() => grimoireService.GetExecutionGroups().ToList());
        }

        public async Task RemoveExecutionGroup(string name)
        {
            await Task.Run(() => grimoireService.RemoveExecutionGroup(name));
        }

        public async Task SaveExecutionGroup(ExecutionGroup executionGroup)
        {
            await Task.Run(() => grimoireService.SaveExecutionGroup(executionGroup));
        }

        public Task<GrimoireScriptBlock> GetScriptBlock(string name)
        {
            return Task.Run(() => grimoireService.GetScriptBlock(name));
        }

        public Task<string> getScriptFullPath(GrimoireScriptBlock scriptBlock)
        {
            return Task.Run(() => grimoireService.getScriptFullPath(scriptBlock));
        }

        public async Task<string> ReadScript(GrimoireScriptBlock scriptBlocks)
        {
            string scriptPath = await getScriptFullPath(scriptBlocks);
            string result = GetScriptText(scriptPath);
            return ExecuteVaultReplacement(result);
        }

        public async Task<IList<Input>> GetInputs(GrimoireScriptBlock scriptBlock)
        {
            Regex regex = new Regex("\\[\\[(.*?)\\]\\]");

            string script = await ReadScript(scriptBlock);

            return regex.Matches(script)
                .Select(match => ConvertInput(match.Value))
                .Distinct()
                .ToList();
        }

        private Input ConvertInput(string inputString)
        {
            return new Input()
            {
                Key = inputString,
                Name = inputString.Substring(2, inputString.Length - 4).Replace("*", ""),
                IsPassword = inputString.Substring(2, inputString.Length - 4).EndsWith("*"),
                Value = ""
            };
        }

        private static string GetScriptText(string scriptPath)
        {
            string result = "";
            using (StreamReader file = new StreamReader(scriptPath))
            {
                result = file.ReadToEnd();
            }
            return result;
        }

        #endregion Script

        #region Runners

        public async Task LoadScriptRunners()
        {
            IEnumerable<GrimoireScriptBlock> scripts = await GetScriptBlocks();
            ScriptRunners = await Task.Run(() =>
            {
                return scripts
                    .Select<GrimoireScriptBlock, IGrimoireRunner>(s => new GrimoireRunner(s, this))
                    .OrderBy(script => script.ScriptBlock.Group)
                    .ThenBy(script => script.ScriptBlock.Name)
                    .ToList();
            });
        }

        #endregion Runners

        #region Configuration

        public async Task<GrimoireConfig> GetConfig()
        {
            Config = await grimoireService.GetConfig();
            return Config;
        }

        public Task SaveConfig(GrimoireConfig config)
        {
            return grimoireService.SaveConfig(config);
        }

        #endregion Configuration

        #region Pin

        public async Task<bool> CheckPin(string pin)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(pin))
            {
                if (await vaultService.CheckPin(pin))
                {
                    this.pin = pin;
                    result = true;
                }
            }
            else if (string.IsNullOrEmpty(pin) && IsDefaultPin)
            {
                result = true;
            }
            return result;
        }

        public Task<bool> ClearPin(string oldPin)
        {
            return ChangePin(oldPin, DEFAULT_PIN);
        }

        public async Task<bool> ChangePin(string oldPin, string pin)
        {
            bool result = false;
            if (await CheckPin(oldPin))
            {
                this.pin = pin;
                await SaveVault();
                result = true;
            }
            return result;
        }

        private void CheckDefaultPin()
        {
            if (vaultService.CheckPin(DEFAULT_PIN).Result)
            {
                pin = DEFAULT_PIN;
            }
        }

        #endregion Pin

        #region Vault

        public async Task ResetVault()
        {
            pin = DEFAULT_PIN;
            await vaultService.ResetVault();
            await LoadVault();
            await SaveVault();
        }

        public async Task SaveVault()
        {
            await vaultService.SaveVault(Vault, pin);
        }

        public async Task LoadVault()
        {
            Vault = await vaultService.GetVault(pin);
        }

        public string GetVaultValue(string key)
        {
            string result = "";
            VaultItem item = Vault.Itens.FirstOrDefault(item =>
                item.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            if (item != null)
            {
                result = item.Value;
            }
            return result;
        }

        public List<string> ExtractKeys(string script)
        {
            Regex regex = new Regex("{{(.*?)}}");
            return regex.Matches(script)
                .Select(match => match.Value).ToList();
        }

        public async Task VerifyKeys(GrimoireScriptBlock scriptBlock)
        {
            string scriptPath = await getScriptFullPath(scriptBlock);
            string script = GetScriptText(scriptPath);
            foreach (string key in ExtractKeys(script))
            {
                VaultItem item = Vault.Itens
                   .FirstOrDefault(item =>
                       $"{{{{{item.Key}}}}}".Equals(key, StringComparison.InvariantCultureIgnoreCase));
                if (item == null)
                {
                    string extractedKey = key.Replace("{{", "").Replace("}}", "");
                    Vault.Itens.Add(new VaultItem(true, extractedKey, ""));
                }
            }
            await SaveVault();
        }

        #endregion Vault

        #region Private

        private string ExecuteVaultReplacement(string script)
        {
            foreach (string key in ExtractKeys(script))
            {
                VaultItem item = Vault.Itens
                    .FirstOrDefault(item =>
                        $"{{{{{item.Key}}}}}".Equals(key, StringComparison.InvariantCultureIgnoreCase));
                string value = item != null ? item.Value : "";
                script = script.Replace(key, value);
            }

            return script;
        }

        #endregion Private
    }
}
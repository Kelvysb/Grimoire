using System.Collections.Generic;
using System.Threading.Tasks;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Business
{
    public interface IGrimoireBusiness
    {
        public IEnumerable<IGrimoireRunner> ScriptRunners { get; set; }

        public bool IsDefaultPin { get; }

        public Vault Vault { get; set; }

        GrimoireConfig Config { get; set; }

        Task ChangeScripBlockOrder(GrimoireScriptBlock scriptBlock, int orderShift);

        Task LoadScriptRunners();

        Task<List<GrimoireScriptBlock>> GetScriptBlocks();

        Task<GrimoireConfig> GetConfig();

        Task SaveConfig(GrimoireConfig config);

        Task<GrimoireScriptBlock> GetScriptBlock(string name);

        Task<List<ExecutionGroup>> GetExecutionGroups();

        Task RemoveScriptBlock(string name);

        Task RemoveExecutionGroup(string name);

        Task SaveScriptBlock(GrimoireScriptBlock scriptBlock);

        Task VerifyKeys(GrimoireScriptBlock scriptBlock);

        Task SaveExecutionGroup(ExecutionGroup executionGroup);

        Task<string> getScriptFullPath(GrimoireScriptBlock scriptBlock);

        Task<bool> CheckPin(string pin);

        Task<bool> ChangePin(string oldPin, string pin);

        Task ResetVault();

        Task SaveVault();

        Task LoadVault();

        Task<bool> ClearPin(string oldPin);

        Task<string> ReadScript(GrimoireScriptBlock scriptBlock);

        string GetVaultValue(string key);

        Task<IList<Input>> GetInputs(GrimoireScriptBlock scriptBlock);
    }
}
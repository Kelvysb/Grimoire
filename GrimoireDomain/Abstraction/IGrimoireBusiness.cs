using System.Collections.Generic;
using System.Threading.Tasks;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Business
{
    public interface IGrimoireBusiness
    {
        public IEnumerable<IGrimoireRunner> ScriptRunners { get; set; }

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

        Task SaveExecutionGroup(ExecutionGroup executionGroup);

        Task<string> getScriptFullPath(GrimoireScriptBlock scriptBlock);
    }
}
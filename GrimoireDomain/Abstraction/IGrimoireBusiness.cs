using System.Collections.Generic;
using System.Threading.Tasks;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Business
{
    public interface IGrimoireBusiness
    {
        Task ChangeScripBlockOrder(GrimoireScriptBlock scriptBlock, int orderShift);

        Task<ScriptResult> ExecuteScript(GrimoireScriptBlock scriptBlock);

        Task<List<GrimoireScriptBlock>> GetScriptBlocks();

        Task<GrimoireScriptBlock> GetScriptBlock(string name);

        Task<List<ExecutionGroup>> GetExecutionGroups();

        Task RemoveScriptBlock(string name);

        Task RemoveExecutionGroup(string name);

        Task SaveScriptBlock(GrimoireScriptBlock scriptBlock);

        Task SaveExecutionGroup(ExecutionGroup executionGroup);
    }
}
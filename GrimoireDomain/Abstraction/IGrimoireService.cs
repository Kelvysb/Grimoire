using System.Collections.Generic;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Services
{
    public interface IGrimoireService
    {
        ICollection<GrimoireScriptBlock> GetScriptBlocks();

        ICollection<ExecutionGroup> GetExecutionGroups();

        void RemoveScriptBlock(string name);

        void RemoveExecutionGroup(string name);

        void SaveScriptBlocks(GrimoireScriptBlock scriptBlock);

        void SaveExecutionGroup(ExecutionGroup executionGroup);

        string getScriptFullPath(GrimoireScriptBlock scriptBlock);
    }
}
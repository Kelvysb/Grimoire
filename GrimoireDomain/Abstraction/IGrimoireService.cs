using System.Collections.Generic;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Services
{
    public interface IGrimoireService
    {
        ICollection<ScriptBlock> GetScriptBlocks();

        ICollection<ExecutionGroup> GetExecutionGroups();

        void RemoveScriptBlock(string name);

        void RemoveExecutionGroup(string name);

        void SaveScriptBlocks(ScriptBlock scriptBlock);

        void SaveExecutionGroup(ExecutionGroup executionGroup);
    }
}
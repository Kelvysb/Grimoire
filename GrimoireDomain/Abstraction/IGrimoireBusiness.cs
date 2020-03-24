using System.Collections.Generic;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Business
{
    public interface IGrimoireBusiness
    {
        void ChangeScripBlockOrder(ScriptBlock scriptBlock, int orderShift);

        ICollection<string> ExecuteScript(ScriptBlock scriptBlock);

        ICollection<ScriptBlock> GetScriptBlocks();

        ICollection<ExecutionGroup> GetExecutionGroups();

        void RemoveScriptBlock(string name);

        void RemoveExecutionGroup(string name);

        void SaveScripBlock(ScriptBlock scriptBlock);

        void SaveExecutionGroup(ExecutionGroup executionGroup);
    }
}
using System.Collections.Generic;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Business
{
    public interface IGrimoireBusiness
    {
        void ChangeScripBlockOrder(ScriptBlock scriptBlock, int orderShift);

        List<string> ExecuteScript(ScriptBlock scriptBlock);

        List<ScriptBlock> GetScriptBlocks();

        List<ExecutionGroup> GetExecutionGroups();

        void RemoveScriptBlock(string name);

        void RemoveExecutionGroup(string name);

        void SaveScripBlock(ScriptBlock scriptBlock);

        void SaveExecutionGroup(ExecutionGroup executionGroup);
    }
}
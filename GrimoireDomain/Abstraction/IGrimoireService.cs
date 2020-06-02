﻿using System.Collections.Generic;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Services
{
    public interface IGrimoireService
    {
        IEnumerable<GrimoireScriptBlock> GetScriptBlocks();

        IEnumerable<ExecutionGroup> GetExecutionGroups();

        void RemoveScriptBlock(string name);

        void RemoveExecutionGroup(string name);

        void SaveScriptBlocks(GrimoireScriptBlock scriptBlock);

        void SaveExecutionGroup(ExecutionGroup executionGroup);

        string getScriptFullPath(GrimoireScriptBlock scriptBlock);

        GrimoireScriptBlock GetScriptBlock(string name);
    }
}
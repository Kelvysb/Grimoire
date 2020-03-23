﻿using System.Collections.Generic;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Business
{
    public interface IGrimoireBusiness
    {
        void ChangeScripBlockOrder(ScriptBlock scriptBlock, int orderShift);

        ICollection<string> ExecuteScript(ScriptBlock scriptBlock);

        ICollection<ScriptBlock> GetScriptBlocks();

        void RemoveScriptBlock(string name);

        void SaveScripBlock(ScriptBlock scriptBlock);
    }
}
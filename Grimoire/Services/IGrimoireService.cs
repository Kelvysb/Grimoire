using System.Collections.Generic;
using Grimoire.Models;

namespace Grimoire.Services
{
    public interface IGrimoireService
    {
        ICollection<ScriptBlock> GetScriptBlocks();

        void RemoveScriptBlock(string name);

        void SaveScriptBlocks(ScriptBlock scriptBlock);
    }
}
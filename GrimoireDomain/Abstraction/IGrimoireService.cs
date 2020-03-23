using System.Collections.Generic;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Services
{
    public interface IGrimoireService
    {
        ICollection<ScriptBlock> GetScriptBlocks();

        void RemoveScriptBlock(string name);

        void SaveScriptBlocks(ScriptBlock scriptBlock);
    }
}
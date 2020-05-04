using System;
using Grimoire.Domain.Models;

namespace Grimoire.ConsoleUI.Helpers
{
    internal class ScriptClickEventArgs : EventArgs
    {
        public ScriptClickEventArgs(GrimoireScriptBlock scriptBlock, ScriptActions action)
        {
            ScriptBlock = scriptBlock;
            Action = action;
        }

        public ScriptActions Action { get; private set; }

        public GrimoireScriptBlock ScriptBlock { get; private set; }
    }
}
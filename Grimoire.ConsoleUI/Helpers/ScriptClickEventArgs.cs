using System;
using Grimoire.Domain.Abstraction.Business;

namespace Grimoire.ConsoleUI.Helpers
{
    public class ScriptClickEventArgs : EventArgs
    {
        public ScriptClickEventArgs(IGrimoireRunner grimoireRunner)
        {
            ScriptRunner = grimoireRunner;
        }

        public IGrimoireRunner ScriptRunner { get; private set; }
    }
}
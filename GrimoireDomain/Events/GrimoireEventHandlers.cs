using Grimoire.Domain.Models;

namespace Grimoire.Domain.Events
{
    public delegate void StartHandler(GrimoireScriptBlock sender);

    public delegate void FinishHandler(GrimoireScriptBlock sender, ScriptResult result);

    public delegate void TimerHandler(GrimoireScriptBlock sender, int timeLeft);
}
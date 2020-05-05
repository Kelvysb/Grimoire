using Grimoire.Domain.Models;

namespace Grimoire.Domain.Events
{
    public delegate void StartHandler(object sender);

    public delegate void FinishHandler(object sender, ScriptResult result);

    public delegate void TimerHandler(object sender, int timeLeft);
}
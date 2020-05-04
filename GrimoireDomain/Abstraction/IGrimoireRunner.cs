using System.Threading.Tasks;
using Grimoire.Domain.Events;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Business
{
    public interface IGrimoireRunner
    {
        GrimoireScriptBlock ScriptBlock { get; }

        bool IsRunning { get; }

        event FinishHandler Finish;

        event StartHandler Start;

        event TimerHandler Timer;

        Task<ScriptResult> Run();
    }
}
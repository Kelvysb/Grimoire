﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Grimoire.Domain.Events;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Business
{
    public interface IGrimoireRunner
    {
        GrimoireScriptBlock ScriptBlock { get; }

        bool IsRunning { get; }

        bool Selected { get; set; }

        bool Paused { get; set; }

        event FinishHandler Finish;

        event StartHandler Start;

        event TimerHandler Timer;

        Task<ScriptResult> Run();

        Task<ScriptResult> Run(IList<Input> inputs);

        Task<IList<Input>> GetInputs();
    }
}
﻿using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Services
{
    public interface IConfigurationService
    {
        GrimoireConfig Config { get; }

        string ExecutionGroupsDirectory { get; }

        string ScriptsDirectory { get; }

        string WorkDirectory { get; }

        void SaveConfig();
    }
}
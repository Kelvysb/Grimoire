using Grimoire.Models;

namespace Grimoire.Services
{
    public interface IConfigurationService
    {
        GrimoireConfig Config { get; }
        string WorkDirectory { get; }

        void SaveConfig();
    }
}
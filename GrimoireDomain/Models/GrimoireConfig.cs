namespace Grimoire.Domain.Models
{
    public class GrimoireConfig
    {
        public string Theme { get; set; }

        public string DefaultScriptEditor { get; set; }

        public string BashPath { get; set; }

        public bool ShowGroup { get; set; } = true;
    }
}
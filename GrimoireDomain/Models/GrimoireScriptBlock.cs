using System.Collections.Generic;

namespace Grimoire.Domain.Models
{
    public class GrimoireScriptBlock
    {
        public string Group { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ScriptType ScriptType { get; set; }

        public AlertLevel AlertLevel { get; set; }

        public string Script { get; set; }

        public string OriginalScriptPath { get; set; }

        public string SuccessPatern { get; set; }

        public PatternRange ExtractResult { get; set; }

        public int Interval { get; set; }

        public int TimeOut { get; set; }

        public ICollection<GrimoireScriptBlock> ScriptBlocks { get; set; }
    }
}
using System.Collections.Generic;

namespace Grimoire.Domain.Models
{
    public class ScriptBlock
    {
        public string Group { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ScriptType ScriptType { get; set; }

        public string File { get; set; }

        public string SuccessPatern { get; set; }

        public AlertLevel AlertLevel { get; set; }

        public int Interval { get; set; }

        public ICollection<ScriptBlock> ScriptBlocks { get; set; }
    }
}
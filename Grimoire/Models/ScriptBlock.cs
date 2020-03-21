using System.Collections.Generic;

namespace Grimoire.Models
{
    public class ScriptBlock
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public ScriptType ScriptType { get; set; }
        
        public string File { get; set; }

        public string SuccessPatern { get; set; }

        public int Interval { get; set; }

        public ICollection<ScriptBlock> ScriptBlocks { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json.Serialization;

namespace Grimoire.Domain.Models
{
    public class GrimoireScriptBlock
    {
        [Required]
        [StringLength(30, ErrorMessage = "Group name too long")]
        public string Group { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Name too long")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Description too long")]
        public string Description { get; set; }

        [Required]
        public ScriptType ScriptType { get; set; }

        [Required]
        public AlertLevel AlertLevel { get; set; }

        [Required]
        public string Script { get; set; }

        [JsonIgnore]
        public Stream OriginalScriptFile { get; set; }

        public string SuccessPatern { get; set; }

        public PatternRange ExtractResult { get; set; }

        public ExecutionMode ExecutionMode { get; set; }

        public int Interval { get; set; }

        [Required]
        public int TimeOut { get; set; }

        public ICollection<GrimoireScriptBlock> ScriptBlocks { get; set; }

        public ScriptResult LastResult { get; set; }
    }
}
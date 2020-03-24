using System.Collections.Generic;

namespace Grimoire.Domain.Models
{
    public class ExecutionGroup
    {
        public int Order { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<string> Scripts { get; set; }
    }
}
using System.IO;

namespace Grimoire.Domain.Models
{
    public class AdditionalFile
    {
        public AdditionalFile(string name, Stream file)
        {
            Name = name;
            File = file;
        }

        public string Name { get; set; }

        public Stream File { get; set; }
    }
}
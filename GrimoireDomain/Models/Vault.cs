using System.Collections.Generic;
using System.Linq;

namespace Grimoire.Domain.Models
{
    public class Vault
    {
        public ICollection<VaultItem> Itens { get; set; } = new List<VaultItem>();

        public Vault Clone()
        {
            return new Vault()
            {
                Itens = Itens.Select(item => item.Clone()).ToList()
            };
        }
    }
}
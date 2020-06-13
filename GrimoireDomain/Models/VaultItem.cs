using System.Text.Json;

namespace Grimoire.Domain.Models
{
    public class VaultItem
    {
        public VaultItem()
        {
            IsSecret = true;
            Key = "";
            Value = "";
        }

        public VaultItem(bool isSecret, string key, string value)
        {
            IsSecret = isSecret;
            Key = key;
            Value = value;
        }

        internal VaultItem Clone()
        {
            return JsonSerializer.Deserialize<VaultItem>(
                JsonSerializer.Serialize(this));
        }

        public bool IsSecret { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
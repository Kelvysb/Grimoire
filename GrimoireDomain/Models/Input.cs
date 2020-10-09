using System;
using System.Diagnostics.CodeAnalysis;

namespace Grimoire.Domain.Models
{
    public class Input : IEquatable<Input>
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsPassword { get; set; }

        public bool Equals([AllowNull] Input other)
        {
            return Key.Equals(other.Key, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
using System.Threading.Tasks;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Services
{
    public interface IVaultService
    {
        Task SaveVault(Vault vault, string pin);

        Task<Vault> GetVault(string pin);

        Task<bool> CheckPin(string pin);

        Task ResetVault();
    }
}
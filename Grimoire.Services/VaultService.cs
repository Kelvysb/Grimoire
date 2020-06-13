using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Services
{
    public class VaultService : IVaultService
    {
        private readonly string publicVaultFile;
        private readonly string privateVaultFile;

        public VaultService(IConfigurationService configurationService)
        {
            publicVaultFile = Path.Combine(configurationService.WorkDirectory, "publicVault.vf");
            privateVaultFile = Path.Combine(configurationService.WorkDirectory, "privateVault.vf");
        }

        public async Task<bool> CheckPin(string pin)
        {
            return await GetVault(pin) != null;
        }

        public async Task<Vault> GetVault(string pin)
        {
            Vault publicVault = null;
            Vault privateVault = new Vault();
            await GrantVaultFile(pin);
            try
            {
                using (StreamReader file = new StreamReader(publicVaultFile))
                {
                    string vaultText = file.ReadToEnd();
                    publicVault = JsonSerializer.Deserialize<Vault>(vaultText);
                }
                if (File.Exists(privateVaultFile))
                {
                    using (StreamReader file = new StreamReader(privateVaultFile))
                    {
                        string vaultText = file.ReadToEnd();
                        vaultText = Decrypt(vaultText, pin);
                        privateVault = JsonSerializer.Deserialize<Vault>(vaultText);
                    }
                }
                return MergeVault(publicVault, privateVault);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task SaveVault(Vault vault, string pin)
        {
            await Task.Run(() =>
            {
                using (StreamWriter file = new StreamWriter(publicVaultFile, false))
                {
                    string publicVaultText = JsonSerializer.Serialize(GetPublicVault(vault));
                    file.Write(publicVaultText);
                }
                using (StreamWriter file = new StreamWriter(privateVaultFile, false))
                {
                    string privateVaultText = Encrypt(JsonSerializer.Serialize(vault), pin);
                    file.Write(privateVaultText);
                }
            });
        }

        public async Task ResetVault()
        {
            await Task.Run(() =>
            {
                if (File.Exists(privateVaultFile))
                {
                    File.Delete(privateVaultFile);
                }
            });
        }

        private async Task GrantVaultFile(string pin)
        {
            if (!File.Exists(publicVaultFile))
            {
                await SaveVault(new Vault(), pin);
            }
        }

        private Vault GetPublicVault(Vault vault)
        {
            Vault result = vault.Clone();
            result.Itens = result.Itens.Select(item =>
            {
                item.Value = "";
                return item;
            }).ToList();
            return result;
        }

        private Vault MergeVault(Vault publicVault, Vault privateVault)
        {
            publicVault.Itens = (from publicItem in publicVault.Itens
                                 join privateItem in privateVault.Itens
                                    on publicItem.Key equals privateItem.Key into result
                                 from item in result.DefaultIfEmpty(publicItem)
                                 select new VaultItem()
                                 {
                                     Key = item.Key,
                                     Value = item.Value,
                                     IsSecret = item.IsSecret
                                 }).ToList();
            return publicVault;
        }

        private string Encrypt(string input, string key)
        {
            byte[] MyEncryptedArray = UTF8Encoding.UTF8
            .GetBytes(input);

            MD5CryptoServiceProvider MyMD5CryptoService = new
               MD5CryptoServiceProvider();

            byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash
               (UTF8Encoding.UTF8.GetBytes(key));

            MyMD5CryptoService.Clear();

            var MyTripleDESCryptoService = new
               TripleDESCryptoServiceProvider();

            MyTripleDESCryptoService.Key = MysecurityKeyArray;

            MyTripleDESCryptoService.Mode = CipherMode.ECB;

            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var MyCrytpoTransform = MyTripleDESCryptoService
               .CreateEncryptor();

            byte[] MyresultArray = MyCrytpoTransform
               .TransformFinalBlock(MyEncryptedArray, 0,
               MyEncryptedArray.Length);

            MyTripleDESCryptoService.Clear();

            return Convert.ToBase64String(MyresultArray, 0,
               MyresultArray.Length);
        }

        private string Decrypt(string input, string key)
        {
            byte[] MyDecryptArray = Convert.FromBase64String
            (input);

            MD5CryptoServiceProvider MyMD5CryptoService = new
               MD5CryptoServiceProvider();

            byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash
               (UTF8Encoding.UTF8.GetBytes(key));

            MyMD5CryptoService.Clear();

            var MyTripleDESCryptoService = new
               TripleDESCryptoServiceProvider();

            MyTripleDESCryptoService.Key = MysecurityKeyArray;

            MyTripleDESCryptoService.Mode = CipherMode.ECB;

            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var MyCrytpoTransform = MyTripleDESCryptoService
               .CreateDecryptor();

            byte[] MyresultArray = MyCrytpoTransform
               .TransformFinalBlock(MyDecryptArray, 0,
               MyDecryptArray.Length);

            MyTripleDESCryptoService.Clear();

            return UTF8Encoding.UTF8.GetString(MyresultArray);
        }
    }
}
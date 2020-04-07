using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Grimoire.Services
{
    public abstract class ResourceLoaderBase
    {
        protected void EnsurePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        protected void SaveResource(object resource, string resourcePath)
        {
            using (StreamWriter writer = new StreamWriter(resourcePath, false))
            {
                writer.Write(JsonSerializer.Serialize(resource));
                writer.Close();
            }
        }

        protected ICollection<T> GetResources<T>(string resourceFolderPath)
        {
            EnsurePath(resourceFolderPath);
            List<T> result = new List<T>();
            List<string> files = Directory.GetFiles(resourceFolderPath).ToList();
            if (files.Any())
            {
                result = files.Select(file => GetResourceFile<T>(file)).ToList();
            }
            return result;
        }

        protected Stream LoadScriptStream(string path)
        {
            Stream result = new MemoryStream();
            using (StreamReader file = new StreamReader(path))
            {
                file.BaseStream.CopyTo(result);
                file.Close();
            }
            return result;
        }

        protected T GetResource<T>(string resourceFolderPath, string name)
        {
            EnsurePath(resourceFolderPath);
            T result = default(T);
            List<string> files = Directory.GetFiles(resourceFolderPath).ToList();
            result = files
                .Where(file => file.StartsWith(name, System.StringComparison.InvariantCultureIgnoreCase))
                .Select(file => GetResourceFile<T>(file))
                .FirstOrDefault();
            return result;
        }

        protected T GetResourceFile<T>(string resourcePath)
        {
            T result = default(T);
            if (File.Exists(resourcePath))
            {
                using (StreamReader reader = new StreamReader(resourcePath))
                {
                    string file = reader.ReadToEnd();
                    reader.Close();
                    return JsonSerializer.Deserialize<T>(file);
                }
            }
            return result;
        }

        protected void RemoveResource(string resourcePath)
        {
            if (File.Exists(resourcePath))
            {
                File.Delete(resourcePath);
            }
            string resourceDirectory = Path.Combine(Path.GetDirectoryName(resourcePath), Path.GetFileNameWithoutExtension(resourcePath));
            if (Directory.Exists(resourceDirectory))
            {
                Directory.Delete(resourceDirectory, true);
            }
        }

        protected string CleanResourceName(string name)
        {
            return RemoveSpecialCharacters(name)
                   .Replace(" ", "_");
        }

        private string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
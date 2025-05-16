using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xeovis.Persistence.Proxies;

namespace Xeovis.Persistence
{
    public static class ProjectFileManager
    {
        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Converters = new List<JsonConverter>() { new StringEnumConverter(), },
        };

        public static bool Save(ProjectFile projectFile, string fullName)
        {
            projectFile.CreatedAt = DateTime.Now;
            var persistable = new ProjectFile01(projectFile);
            var json = JsonConvert.SerializeObject(persistable, _jsonSettings);

            try
            {
                File.WriteAllText(fullName, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static ProjectFile Load(string fullName)
        {
            try
            {
                var json = File.ReadAllText(fullName);
                using var doc = JsonDocument.Parse(json);
                int version = doc.RootElement.GetProperty("Version").GetInt32();

                return version switch
                {
                    1 => FromV01(json),
                    _ => throw new NotSupportedException("Unknown version")
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static ProjectFile FromV01(string json)
        {
            var persistable = JsonConvert.DeserializeObject<ProjectFile01>(json, _jsonSettings);
            return persistable.ConvertToRealObject();
        }
    }
}
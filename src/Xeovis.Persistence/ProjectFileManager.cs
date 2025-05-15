using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xeovis.Persistence.Proxies;

namespace Xeovis.Persistence
{
    public static class ProjectFileManager
    {
        public static bool Save(ProjectFile projectFile, string fullName)
        {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            jsonSettings.Converters.Add(new StringEnumConverter());

            projectFile.CreatedAt = DateTime.Now;
            var persistable = new ProjectFile01(projectFile);
            var json = JsonConvert.SerializeObject(persistable, jsonSettings);

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
            var persistable = JsonConvert.DeserializeObject<ProjectFile01>(json);
            return persistable.ConvertToRealObject();
        }
    }
}
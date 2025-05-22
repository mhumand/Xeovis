using System.IO.Compression;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
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
                // File.WriteAllText(fullName, json);
                ZipTextContent(json, fullName, Path.GetFileNameWithoutExtension(fullName) + ".json");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to save project file: {ex}", ex.Message);
                return false;
            }
        }

        public static ProjectFile Load(string fullName)
        {
            try
            {
                // var json = File.ReadAllText(fullName);
                var json = ReadTextFromZip(fullName, Path.GetFileNameWithoutExtension(fullName) + ".json");
                using var doc = JsonDocument.Parse(json);
                int version = doc.RootElement.GetProperty("Version").GetInt32();

                return version switch
                {
                    1 => FromV01(json),
                    _ => throw new NotSupportedException()
                };
            }
            catch (Exception ex)
            {
                if (ex is NotSupportedException)
                    Log.Error("Failed to load unknown project file version.");
                else
                    Log.Error("Failed to load project file: {ex}", ex.Message);

                return null;
            }
        }

        public static void ZipTextContent(string textContent, string zipPath, string entryNameInZip)
        {
            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    var entry = archive.CreateEntry(entryNameInZip);
                    using (var entryStream = entry.Open())
                    using (var writer = new StreamWriter(entryStream))
                    {
                        writer.Write(textContent);
                    }
                }
            }
        }

        public static string ReadTextFromZip(string zipPath, string entryName)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                var entry = archive.GetEntry(entryName);
                if (entry == null)
                    throw new FileNotFoundException("Entry not found in zip.");

                using (var reader = new StreamReader(entry.Open()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static ProjectFile FromV01(string json)
        {
            var persistable = JsonConvert.DeserializeObject<ProjectFile01>(json, _jsonSettings);
            return persistable.ConvertToRealObject();
        }
    }
}
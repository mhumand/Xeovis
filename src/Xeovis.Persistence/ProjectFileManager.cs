using Newtonsoft.Json;

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

            projectFile.CreatedAt = DateTime.Now;
            var json = JsonConvert.SerializeObject(projectFile, jsonSettings);

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
            var json = File.ReadAllText(fullName);

            try
            {
                var projectFile = JsonConvert.DeserializeObject<ProjectFile>(json);
                return projectFile;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}

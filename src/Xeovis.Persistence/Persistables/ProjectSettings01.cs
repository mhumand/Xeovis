using Newtonsoft.Json;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Persistence.Persistables;

namespace Xeovis.Persistence
{
    public class ProjectSettings01
    {
        public List<PhaseNameSet01> AllowedPhaseNames { get; set; }
        public Dictionary<Guid, List<xeosVariableName01>> PhaseXeosVariables { get; set; }

        [JsonProperty("PhaseOxides")]
        public Dictionary<Guid, List<string>> PhaseComponents { get; set; }

        [JsonProperty("xeosVariableCations")]
        public Dictionary<Guid, List<string>> xeosVariableElements { get; set; }

        public List<string> Elements { get; set; }
        public Dictionary<string, List<string>> Components { get; set; }

        public ProjectSettings01()
        {
            
        }

        public ProjectSettings01(ProjectSettings realObject)
        {
            AllowedPhaseNames = realObject.AllowedPhaseNames.Select(x => new PhaseNameSet01(x)).ToList();

            PhaseXeosVariables = realObject.PhaseXeosVariables.ToDictionary(
                k => k.Key,
                v => v.Value.Select(x => new xeosVariableName01(x))
                    .ToList());

            PhaseComponents = realObject.PhaseComponents;
            xeosVariableElements = realObject.xeosVariableElements;

            Elements = realObject.Elements;
            Components = realObject.Components;
        }

        public ProjectSettings ConvertToRealObject()
        {
            return new ProjectSettings()
            {
                AllowedPhaseNames = AllowedPhaseNames.Select(x => x.ConvertToRealObject()).ToList(),
                PhaseXeosVariables = PhaseXeosVariables.ToDictionary(
                    o => o.Key,
                    o => o.Value.Select(x => x.ConvertToRealObject())
                        .ToList()),

                PhaseComponents = PhaseComponents,
                xeosVariableElements = xeosVariableElements,

                Elements = Elements,
                Components = Components
            };
        }
    }
}
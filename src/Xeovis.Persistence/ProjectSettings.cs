using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Domain.Definitions.MCTC;

namespace Xeovis.Persistence
{
    public class ProjectSettings
    {
        public List<PhaseNameSet> AllowedPhaseNames { get; set; }
        public Dictionary<Guid, List<xeosVariableName>> PhaseXeosVariables { get; set; }
        public Dictionary<Guid, List<string>> PhaseComponents { get; set; }
        public Dictionary<Guid, List<string>> xeosVariableElements { get; set; }
        
        public List<string> Elements { get; set; }
        public Dictionary<string, List<string>> Components { get; set; }

        public ProjectSettings()
        {
            AllowedPhaseNames = PhaseNameList.XeovisDefaults;
            PhaseXeosVariables = xeosVariableNameList.XeovisDefaults;
            PhaseComponents = PhaseComponentList.XeovisDefaults.ToDictionary(
                k => k.Key,
                v => v.Value.Select(x => x.Formula).ToList());
            xeosVariableElements = xeosVariableElementList.XeovisDefaults.ToDictionary(
                k => k.Key,
                v => v.Value.Select(x => x.Symbol).ToList());

            Elements = new();
            Components = new();
        }
    }
}

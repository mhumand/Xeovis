using Xeovis.Domain;
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
        public Dictionary<Guid, List<OxideType>> PhaseOxides { get; set; }
        public Dictionary<Guid, List<CationType>> xeosVariableCations { get; set; }

        public ProjectSettings()
        {
            AllowedPhaseNames = PhaseNameList.XeovisDefaults;
            PhaseXeosVariables = xeosVariableNameList.XeovisDefaults;
            PhaseOxides = PhaseOxideList.XeovisDefaults;
            xeosVariableCations = xeosVariableCationList.XeovisDefaults;
        }
    }
}

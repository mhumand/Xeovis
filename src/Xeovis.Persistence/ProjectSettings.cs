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
        public Dictionary<Guid, List<Component>> PhaseComponents { get; set; }
        public Dictionary<Guid, List<Element>> xeosVariableElements { get; set; }
        
        public List<Element> CustomElements { get; set; }
        public List<Component> CustomComponents { get; set; }

        public ProjectSettings()
        {
            AllowedPhaseNames = PhaseNameList.XeovisDefaults;
            PhaseXeosVariables = xeosVariableNameList.XeovisDefaults;
            PhaseComponents = PhaseComponentList.XeovisDefaults;
            xeosVariableElements = xeosVariableElementList.XeovisDefaults;
            
            CustomElements = new List<Element>();
            CustomComponents = new List<Component>();
        }
    }
}

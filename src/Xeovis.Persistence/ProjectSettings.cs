using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;

namespace Xeovis.Persistence
{
    public class ProjectSettings
    {
        public List<PhaseNameSet> AllowedPhaseNames { get; set; }

        public ProjectSettings()
        {
            AllowedPhaseNames = PhaseNameList.XeovisDefaults;
            PhaseNameList.PhaseNameListChanged += PhaseNameList_PhaseNameListChanged;
        }

        private void PhaseNameList_PhaseNameListChanged(object? sender, Domain.Definitions.Globals.PhaseNameListChangedEventArgs e)
        {
            AllowedPhaseNames = e.NameSets;
        }
    }
}

using System.Collections.Generic;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;

namespace Xeovis.Persistence
{
    public class ProjectSettings
    {
        private List<PhaseNameSet> _allowedPhaseNames { get; set; }
        public List<PhaseNameSet> AllowedPhaseNames
        {
            get => _allowedPhaseNames;
            set
            {
                _allowedPhaseNames = value;
                PhaseNameList.SetPhaseNames(value, invokeEvent: true);
            }
        }

        public ProjectSettings()
        {
        }
    }
}

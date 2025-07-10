using Xeovis.Domain.Definitions.Experiment;

namespace Xeovis.Domain.Definitions.Globals
{
    public class PhaseNameListChangedEventArgs : EventArgs
    {
        public List<PhaseNameSet> NameSets { get; set; }
        public PhaseNameListChangedEventArgs(List<PhaseNameSet> nameSets)
        {
            NameSets = nameSets;
        }
    }
}

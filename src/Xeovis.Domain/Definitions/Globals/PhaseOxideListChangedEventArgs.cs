namespace Xeovis.Domain.Definitions.Globals
{
    public class PhaseOxideListChangedEventArgs : EventArgs
    {
        public Dictionary<Guid, List<OxideType>> PhaseOxideList;

        public PhaseOxideListChangedEventArgs(Dictionary<Guid, List<OxideType>> phaseOxideList)
        {
            PhaseOxideList = phaseOxideList;
        }
    }
}

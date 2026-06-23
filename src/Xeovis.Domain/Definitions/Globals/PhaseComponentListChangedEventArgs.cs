namespace Xeovis.Domain.Definitions.Globals
{
    public class PhaseComponentListChangedEventArgs : EventArgs
    {
        public Dictionary<Guid, List<Component>> PhaseComponentList;

        public PhaseComponentListChangedEventArgs(Dictionary<Guid, List<Component>> phaseComponentList)
        {
            PhaseComponentList = phaseComponentList;
        }
    }
}

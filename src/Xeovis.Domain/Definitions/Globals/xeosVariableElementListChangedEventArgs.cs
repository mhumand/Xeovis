namespace Xeovis.Domain.Definitions.Globals
{
    public class xeosVariableElementListChangedEventArgs : EventArgs
    {
        public Dictionary<Guid, List<Element>> ElementList { get; set; }

        public xeosVariableElementListChangedEventArgs(Dictionary<Guid, List<Element>> elementList)
        {
            ElementList = elementList;
        }
    }
}

namespace Xeovis.Domain.Definitions.Globals
{
    public class xeosVariableCationListChangedEventArgs : EventArgs
    {
        public Dictionary<Guid, List<CationType>> CationList { get; set; }

        public xeosVariableCationListChangedEventArgs(Dictionary<Guid, List<CationType>> cationList)
        {
            CationList = cationList;
        }
    }
}

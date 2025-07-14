using Xeovis.Domain.Definitions.MCTC;

namespace Xeovis.Domain.Definitions.Globals
{
    public class xeosVariableNameListChangedEventArgs : EventArgs
    {
        public Dictionary<Guid, List<xeosVariableName>> xeosVariableNameList { get; set; }

        public xeosVariableNameListChangedEventArgs(Dictionary<Guid, List<xeosVariableName>> xeosVariableNameList)
        {
            this.xeosVariableNameList = xeosVariableNameList;
        }
    }
}

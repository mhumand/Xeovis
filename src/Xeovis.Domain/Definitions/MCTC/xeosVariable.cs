using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Domain.Definitions.MCTC;

namespace Xeovis.Domain
{
    public class xeosVariable
    {
        public string Name => $"{VariableName}({PhaseKeyName})";
        public double ObsVal { get; set; }
        public Guid Id { get; set; }
        public Dictionary<int, double> CalcVals { get; set; } // int being the index of the results group
        public Guid BatchId { get; set; }
        public Guid mctcBundleId { get; set; }
        public Guid PhaseNameId { get; set; }
        public PhaseNameSet PhaseNameSet => PhaseNameList.GetNameSetById(PhaseNameId);
        public string PhaseKeyName => PhaseNameSet?.KeyName;
        public Guid NameId { get; set; }
        public xeosVariableName VariableName => xeosVariableNameList.GetXeosVariableNameById(NameId);
        public double DefaultValue { get; set; }

        public xeosVariable()
        {
            CalcVals = new Dictionary<int, double>();
            Id = Guid.NewGuid();
        }

        public bool IsEqualTo(xeosVariable otherVariable)
        {
            if (otherVariable == null) return false;
            if (otherVariable.VariableName == null) return false;
            if (VariableName == null) return false;

            return PhaseKeyName == otherVariable.PhaseKeyName && VariableName.Name == otherVariable.VariableName.Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

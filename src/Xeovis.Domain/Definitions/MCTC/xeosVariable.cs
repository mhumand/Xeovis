using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;

namespace Xeovis.Domain
{
    public class xeosVariable
    {
        public string Name => $"{VariableType}({PhaseKeyName})";
        public double ObsVal { get; set; }
        public Guid Id { get; set; }
        public Dictionary<int, double> CalcVals { get; set; } // int being the index of the results group
        public Guid BatchId { get; set; }
        public Guid mctcBundleId { get; set; }
        public Guid PhaseNameId { get; set; }
        public PhaseNameSet PhaseNameSet => PhaseNameList.GetNameSetById(PhaseNameId);
        public string PhaseKeyName => PhaseNameSet.KeyName;
        public ModelVariableType VariableType { get; set; }
        public double DefaultValue { get; set; }
        public bool IsOrderVariable => VariableType.IsOrderVariable();

        public xeosVariable()
        {
            CalcVals = new Dictionary<int, double>();
            Id = Guid.NewGuid();
        }

        public bool IsEqualTo(xeosVariable otherVariable)
        {
            return PhaseKeyName == otherVariable.PhaseKeyName && VariableType == otherVariable.VariableType;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

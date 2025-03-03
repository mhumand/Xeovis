using DatabaseReader.Definitions.Persistence;
using DatabaseReader.Enums;
using DatabaseReader.Enums.Modelling;
using System.Collections.Generic;

namespace DatabaseReader.Definitions
{
    public class ModelVariable
    {
        public string Name => $"{VariableType}({Phase})";
        public double ObsVal { get; set; }
        public Dictionary<int, double> CalcVals { get; set; } // int being the index of the results group
        public Batch Batch { get; set; }
        public PhaseType Phase { get; set; }
        public ModelVariableType VariableType { get; set; }
        public double DefaultValue { get; set; }
        public bool IsOrderVariable => ModelVariableTypeExtensions.IsOrderVariable(VariableType);

        public ModelVariable()
        {
            CalcVals =  new Dictionary<int, double>();
        }

        public override string ToString()
        {
            return Name;
        }

        public string ToString(bool includeBatchNumber)
        {
            var batchStr = includeBatchNumber ? $"-b{Batch.Index}" : string.Empty;
            return Name + batchStr;
        }
    }
}

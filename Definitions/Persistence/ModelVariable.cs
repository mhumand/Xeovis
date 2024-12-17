using DatabaseReader.Enums;
using DatabaseReader.Enums.Modelling;

namespace DatabaseReader.Definitions
{
    public class ModelVariable
    {
        public string Name => $"{VariableType}({Phase})";
        public double ObsVal { get; set; }
        public double CalcVal { get; set; }
        public Batch Batch { get; set; }
        public PhaseType Phase { get; set; }
        public ModelVariableType VariableType { get; set; }

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

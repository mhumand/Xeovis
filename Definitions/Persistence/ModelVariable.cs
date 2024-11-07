using DatabaseReader.Enums;

namespace DatabaseReader.Definitions
{
    public class ModelVariable
    {
        public string Name { get; set; }
        public double ObsVal { get; set; }
        public double CalcVal { get; set; }
        public Batch Batch { get; set; }
        public PhaseType Phase { get; set; }

        public override string ToString()
        {
            return $"{Name}({Phase})";
        }

        public string ToString(bool includeBatchNumber)
        {
            var batchStr = includeBatchNumber ? $"-b{Batch.Index}" : string.Empty;
            return $"{Name}({Phase})" + batchStr;
        }
    }
}

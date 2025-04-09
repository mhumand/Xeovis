namespace Xeovis.Domain
{
    public class xeosVariable
    {
        public string Name => $"{VariableType}({PhaseType})";
        public double ObsVal { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public Dictionary<int, double> CalcVals { get; set; } // int being the index of the results group
        public Batch Batch { get; set; }
        public Guid BatchId { get; set; }
        public PhaseType PhaseType { get; set; }
        public ModelVariableType VariableType { get; set; }
        public double DefaultValue { get; set; }
        public bool IsOrderVariable => ModelVariableTypeExtensions.IsOrderVariable(VariableType);

        public xeosVariable()
        {
            CalcVals = new Dictionary<int, double>();
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

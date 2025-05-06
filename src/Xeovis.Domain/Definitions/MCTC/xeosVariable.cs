namespace Xeovis.Domain
{
    public class xeosVariable
    {
        public string Name => $"{VariableType}({PhaseType})";
        public double ObsVal { get; set; }
        public Guid Id { get; set; }
        public Dictionary<int, double> CalcVals { get; set; } // int being the index of the results group
        public Guid BatchId { get; set; }
        public Guid mctcBundleId { get; set; }
        public PhaseType PhaseType { get; set; }
        public ModelVariableType VariableType { get; set; }
        public double DefaultValue { get; set; }
        public bool IsOrderVariable => VariableType.IsOrderVariable();

        public xeosVariable()
        {
            CalcVals = new Dictionary<int, double>();
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

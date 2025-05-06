namespace Xeovis.Domain
{
    public class Batch
    {
        public string Name => $"{Index} - {ExperimentName} - {ExpRunName}";
        public List<PhaseType> Phases { get; set; }
        public int Index { get; set; }
        public Guid Id { get; set; }
        public Guid mctcBundleId { get; set; }

        public string ExperimentName { get; set; }
        public Guid ExperimentId { get; set; }

        public string ExpRunName { get; set; }
        public double ExpRunPressure { get; set; }
        public double ExpRunTemperature { get; set; }
        public Guid ExpRunId { get; set; }

        public List<double> BulkComposition { get; set; }
        public List<OxideType> BulkOxides { get; set; }
        public Dictionary<int, bool> IsFailed { get; set; } // int being the index of the mctc run => shows in which run group the batch has failed
        public List<xeosVariable> xeosVariables { get; set; }

        public Batch()
        {
            Id = Guid.NewGuid();
            ExperimentName = string.Empty;
            ExpRunName = string.Empty;
            Phases = new List<PhaseType>();
            BulkComposition = new List<double>();
            BulkOxides = new List<OxideType>();
            xeosVariables = new List<xeosVariable>();
            IsFailed = new Dictionary<int, bool>();
        }

        public override string ToString()
        {
            return Name;
        }

        public string GetPhaseAssemblageString(string spacer = " ")
        {
            var str = string.Empty;
            foreach (var phase in Phases)
                str += phase + spacer;

            return str.TrimEnd(spacer.ToCharArray());
        }

        public bool IsAllFailed()
        {
            return IsFailed.All(i => i.Value == true);
        }
    }
}

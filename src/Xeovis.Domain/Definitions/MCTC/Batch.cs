using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;

namespace Xeovis.Domain
{
    public class Batch
    {
        public string Name => $"{Index} - {ExperimentName} - {ExpRunName}";
        public List<Guid> PhaseNameIds { get; set; }
        public List<PhaseNameSet> PhaseNameSets => PhaseNameIds.Select(x => PhaseNameList.GetNameSetById(x)).ToList();
        public int Index { get; set; }
        public Guid Id { get; set; }
        public Guid mctcBundleId { get; set; }

        public string ExperimentName { get; set; }
        public Guid ExperimentId { get; set; }

        public string ExpRunName { get; set; }
        public double ExpRunPressure { get; set; }
        public double ExpRunTemperature { get; set; }
        public Guid ExpRunId { get; set; }
        public Dictionary<OxideType, double> BulkComposition { get; set; }

        /// <summary>
        /// int is the index of mctc run => shows in which run group the batch has failed
        /// </summary>
        public Dictionary<int, bool> IsFailed { get; set; }

        public List<xeosVariable> xeosVariables { get; set; }

        public Batch()
        {
            Id = Guid.NewGuid();
            ExperimentName = string.Empty;
            ExpRunName = string.Empty;
            PhaseNameIds = new List<Guid>();
            BulkComposition = new Dictionary<OxideType, double>();
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
            foreach (var phaseName in PhaseNameSets)
                str += phaseName.KeyName + spacer;

            return str.TrimEnd(spacer.ToCharArray());
        }

        public bool IsAllFailed()
        {
            return IsFailed.All(i => i.Value == true);
        }
    }
}
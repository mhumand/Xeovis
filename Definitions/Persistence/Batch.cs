using DatabaseReader.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseReader.Definitions
{
    public class Batch
    {
        public string Name => $"{Index} - {Experiment.Name} - {ExperimentalRun.Name}";
        public List<PhaseType> Phases { get; set; }
        public int Index { get; set; }
        public Experiment Experiment { get; set; }
        public ExperimentalRun ExperimentalRun { get; set; }
        public List<double> BulkComposition { get; set; }
        public List<OxideType> BulkOxides { get; set; }
        public Dictionary<int, bool> IsFailed { get; set; } // int being the index of the results group => shows in which run group the batch has failed
        public List<ModelVariable> ModelVariables { get; set; }

        public Batch()
        {
            Phases = new List<PhaseType>();
            BulkComposition = new List<double>();
            BulkOxides = new List<OxideType>();
            ModelVariables = new List<ModelVariable>();
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

using DatabaseReader.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public bool IsFailed { get; set; }
        public List<ModelVariable> ModelVariables { get; set; }

        public Batch()
        {
            Phases = new List<PhaseType>();
            BulkComposition = new List<double>();
            BulkOxides = new List<OxideType>();
            ModelVariables = new List<ModelVariable>();
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
    }
}

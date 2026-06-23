using Xeovis.Domain.Definitions.Globals;

namespace Xeovis.Domain
{
    public class ExpRun
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public List<Phase> Phases { get; set; }
        public Dictionary<Component, double> BulkComposition { get; set; }
        public List<double> BulkCompositionValues => BulkComposition?.Values.ToList();
        public List<Component> BulkCompositionComponents => BulkComposition?.Keys.ToList();
        public double Pressure { get; set; } //Kbar
        public double ErrorP { get; set; } // Kbar
        public double Temperature { get; set; } // °C
        public double ErrorT { get; set; } // °C
        public double Duration { get; set; }
        public double OxygenFugacity { get; set; }
        public Guid ExperimentId { get; set; }
        public string Laboratory { get; set; }
        public string Device { get; set; }
        public string Container { get; set; }
        public string Method { get; set; }
        public int Index { get; set; }
        public int ReadingIndex { get; set; }
        public RockType Rocktype { get; set; }

        public ExpRun()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Laboratory = string.Empty;
            Device = string.Empty;
            Container = string.Empty;
            Method = string.Empty;
            Phases = new List<Phase>();
            BulkComposition = new Dictionary<Component, double>();
            ReadingIndex = -999;
        }

        public string GetPhaseAssemblageString(List<Phase> excludingPhases = null)
        {
            string phaseString = string.Empty;
            if (excludingPhases == null)
                excludingPhases = new List<Phase>();

            foreach (var phase in Phases.Except(excludingPhases))
            {
                var name = phase.NameSet == null ? "?" : phase.NameSet.KeyName;
                phaseString += name + "+";
            }

            return phaseString.TrimEnd('+');
        }

        public override string ToString()
        {
            string indexStr = Index != 0 ? Index.ToString() + " - " : string.Empty;
            string ptStr = Pressure != 0 && Temperature != 0 ? $" ({Pressure} Kbar, {Temperature} °C)" : string.Empty;
            return indexStr + Name + ptStr;
        }
    }
}
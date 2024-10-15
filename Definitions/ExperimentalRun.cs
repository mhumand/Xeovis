using DatabaseReader.Enums;
using System.Collections.Generic;

namespace DatabaseReader.Definitions
{
    public class ExperimentalRun
    {
        public string Name { get; set; }
        public List<Phase> Phases { get; set; }
        public List<double> BulkComposition {  get; set; }
        public List<OxideType> BulkOxides { get; set; }
        public double Pressure { get; set; } //Kbar
        public double ErrorP { get; set; } // Kbar
        public double Temperature { get; set; } // °C
        public double ErrorT { get; set; } // °C
        public double Duration { get; set; }
        public double OxygenFugacity { get; set; }
        public Experiment Experiment { get; set; }
        public string Laboratory {  get; set; }
        public string Device { get; set; }
        public string Container { get; set; }
        public string Method { get; set; }
        public string MyProperty { get; set; }
        public int Index {  get; set; }

        public ExperimentalRun()
        {
            Phases = new List<Phase>();
            BulkComposition = new List<double>();
            BulkOxides = new List<OxideType>();
        }

        public override string ToString()
        {
            return Index.ToString() + " - " + Name + $"({Pressure} Kbar, {Temperature} °C)";
        }
    }
}

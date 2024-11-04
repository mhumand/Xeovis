using DatabaseReader.Enums;
using System.Collections.Generic;

namespace DatabaseReader.Definitions
{
    public class Phase
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public PhaseType Type { get; set; }
        public PhaseState State { get; set; }
        public List<string> OtherNames { get; set; }
        public List<EndMember> EndMembers { get; set; }
        public List<OxideType> Oxides { get; set; }
        public List<double> Composition { get; set; }
        public double Total {  get; set; }
        public ExperimentalRun ExperimentalRun { get; set; }
        public string Comment { get; set; }

        // phase attributes?


        //

        public Phase(string name)
        {
            Name = name;
            Type = PhaseTypeExtensions.GetPhaseTypeByName(Name);
            Composition = new List<double>();
            Oxides = new List<OxideType>();
            EndMembers = new List<EndMember>();
            OtherNames = new List<string>();
        }

        public object Tag { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

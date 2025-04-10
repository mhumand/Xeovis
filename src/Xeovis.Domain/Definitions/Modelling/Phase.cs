namespace Xeovis.Domain
{
    public class Phase
    {
        public string Name { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public PhaseType Type { get; set; }
        public PhaseState State { get; set; }
        public List<string> OtherNames { get; set; }
        //public List<EndMember> EndMembers { get; set; }
        public List<OxideType> Oxides { get; set; }
        public List<double> Composition { get; set; }
        public ExpRun ExpRun { get; set; }//todo: will be removed
        public Guid ExpRunId { get; set; }
        public string Comment { get; set; }

        // phase attributes?


        //

        public Phase(string name)
        {
            Name = name;
            Type = PhaseTypeExtensions.GetPhaseTypeByName(Name);
            Composition = new List<double>();
            Oxides = new List<OxideType>();
            //EndMembers = new List<EndMember>();
            OtherNames = new List<string>();
        }

        public object Tag { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

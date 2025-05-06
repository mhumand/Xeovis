namespace Xeovis.Domain
{
    public class Phase
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public PhaseType Type { get; set; }
        public PhaseState State { get; set; }
        public List<string> OtherNames { get; set; }
        public List<EndMember> EndMembers { get; set; }
        public List<OxideType> Oxides { get; set; }
        public List<double> Composition { get; set; }
        public Guid ExpRunId { get; set; }
        public string Comment { get; set; }

        public Phase()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Comment = string.Empty;
            Composition = new List<double>();
            Oxides = new List<OxideType>();
            EndMembers = new List<EndMember>();
            OtherNames = new List<string>();
        }

        public Phase(string name) : this()
        {
            Name = name;
            Type = PhaseTypeExtensions.GetPhaseTypeByName(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

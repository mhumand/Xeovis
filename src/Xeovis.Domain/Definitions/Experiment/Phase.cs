namespace Xeovis.Domain
{
    public class Phase
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public PhaseType Type { get; set; }
        public PhaseState State { get; set; }
        public List<EndMember> EndMembers { get; set; }
        public Dictionary<OxideType, double> Composition { get; set; }
        public List<OxideType> CompositionOxides => Composition.Keys.ToList();
        public List<double> CompositionValues => Composition.Values.ToList();
        public Guid ExpRunId { get; set; }
        public string Comment { get; set; }

        public Phase()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Comment = string.Empty;
            Composition = new Dictionary<OxideType, double>();
            EndMembers = new List<EndMember>();
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
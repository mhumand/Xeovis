using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;

namespace Xeovis.Domain
{
    public class Phase
    {
        public Guid NameId { get; set; }
        public PhaseNameSet NameSet => PhaseNameList.GetNameSetById(NameId);
        public Guid Id { get; set; }
        public PhaseState State { get; set; }
        public List<EndMember> EndMembers { get; set; }
        public Dictionary<Component, double> Composition { get; set; }
        public List<Component> CompositionComponents => Composition?.Keys.ToList();
        public List<double> CompositionValues => Composition?.Values.ToList();
        public Guid ExpRunId { get; set; }
        public string Comment { get; set; }

        public Phase()
        {
            NameId = Guid.Empty;
            Id = Guid.NewGuid();
            Comment = string.Empty;
            Composition = new Dictionary<Component, double>();
            EndMembers = new List<EndMember>();
        }

        public Phase(Guid nameId) : this()
        {
            NameId = nameId;
        }
        
        public bool HasName(string name)
        {
            if (NameSet == null) return false;
            return NameSet.HasName(name);
        }

        public override string ToString()
        {
            if (NameSet == null) return string.Empty;
            return NameSet.ToString();
        }
    }
}
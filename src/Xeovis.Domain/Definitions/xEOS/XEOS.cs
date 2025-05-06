namespace Xeovis.Domain
{
    public class XEOS
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public PhaseType PhaseType { get; set; }
        public List<EndMember> EndMembers { get; set; }
        public List<EnergyParameter> Ws { get; set; }
        public List<AlphaParameter> Alphas { get; set; }
        public List<xeosVariable> Variables { get; set; }
        public XEOS()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            EndMembers = new List<EndMember>();
            Ws = new List<EnergyParameter>();
            Alphas = new List<AlphaParameter>();
            Variables = new List<xeosVariable>();
        }

        public EnergyParameter GetWij(EndMember endMember1, EndMember endMember2)
        {
            if (endMember1 == null || endMember2 == null) return null;
            if (Ws == null || Ws.Count == 0) return null;
            var w = Ws.FirstOrDefault(ws => (ws.Endmember1 == endMember1 && ws.Endmember2 == endMember2)
            || (ws.Endmember1 == endMember2 && ws.Endmember2 == endMember1));
            if (w == null) return null;
            return w;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

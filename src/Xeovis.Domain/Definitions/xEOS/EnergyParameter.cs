namespace Xeovis.Domain
{
    public class EnergyParameter
    {
        public EndMember Endmember1 { get; set; }
        public EndMember Endmember2 { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid xeosId { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public override string ToString()
        {
            return $"W({Endmember1.Name},{Endmember2.Name})";
        }
    }
}

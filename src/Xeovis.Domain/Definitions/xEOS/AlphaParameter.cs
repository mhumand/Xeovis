namespace Xeovis.Domain
{
    public class AlphaParameter
    {
        public EndMember Endmember { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid xeosId { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public override string ToString()
        {
            return $"α({Endmember.Name})";
        }
    }
}

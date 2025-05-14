namespace Xeovis.Domain
{
    public class AlphaParameter
    {
        public EndMember Endmember { get; set; }
        public Guid Id { get; set; }
        public Guid xeosId { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public AlphaParameter()
        {
            Id = Guid.NewGuid();
            Endmember = new EndMember();
        }

        public override string ToString()
        {
            return $"α({Endmember.Name})";
        }
    }
}

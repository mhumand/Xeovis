namespace DatabaseReader.Definitions.xEOS
{
    public class AlphaParameter
    {
        public EndMember Endmember { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public override string ToString()
        {
            return $"α({Endmember.Name})";
        }
    }
}

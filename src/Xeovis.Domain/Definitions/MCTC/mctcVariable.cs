namespace Xeovis.Domain
{
    public class mctcVariable
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public Guid Id { get; set; }
        public Guid mctcBundleId { get; set; }
        public List<double> Values { get; set; }
        public double StartingValue { get; set; }
        public double Range { get; set; }

        public mctcVariable()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Values = new List<double>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

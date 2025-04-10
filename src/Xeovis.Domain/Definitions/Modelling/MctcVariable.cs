namespace Xeovis.Domain
{
    public class mctcVariable
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<double> Values { get; set; }
        public double StartingValue { get; set; }
        public double Range { get; set; }

        public mctcVariable()
        {
            Values = new List<double>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

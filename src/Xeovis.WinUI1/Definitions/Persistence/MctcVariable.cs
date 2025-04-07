using System.Collections.Generic;

namespace DatabaseReader.Definitions.Persistence
{
    public class MctcVariable
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public List<double> Values { get; set; }
        public double StartingValue { get; set; }
        public double Range { get; set; }

        public MctcVariable()
        {
            Values = new List<double>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

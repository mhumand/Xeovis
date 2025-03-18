using Xeovis.Base;

namespace Xeovis.Domain
{
    public class Parameter
    {
        public string Name { get; set; }
        public double Value { get; set; }



        public override string ToString()
        {
            return Name + "=" + Value.Round(8).ToString();
        }
    }
}

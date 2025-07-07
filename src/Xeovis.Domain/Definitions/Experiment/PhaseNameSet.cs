namespace Xeovis.Domain.Definitions.Experiment
{
    public class PhaseNameSet
    {
        public string FullName { get; set; }
        public string KeyName { get; set; }
        public List<string> OtherNames { get; set; }
        public Guid Id { get; set; }
        public PhaseNameSet() 
        {
            Id = Guid.NewGuid();
            FullName = string.Empty;
            KeyName = string.Empty;
            OtherNames = new List<string>();
        }

        public PhaseNameSet(string fullName, string keyName) : this()
        {
            FullName = fullName;
            KeyName = keyName;
        }

        public PhaseNameSet(string fullName, string keyName, List<string> otherNames) : this()
        {
            FullName = fullName;
            KeyName = keyName;
            OtherNames = otherNames;
        }

        public bool HasName(string name)
        {
            return FullName.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                || KeyName.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                || OtherNames.Contains(name, StringComparer.InvariantCultureIgnoreCase);
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(FullName) ? KeyName : FullName;
        }
    }
}

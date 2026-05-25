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
            // BuildNameCache();
        }

        public PhaseNameSet(string fullName, string keyName) : this()
        {
            FullName = fullName;
            KeyName = keyName;
            // BuildNameCache();
        }

        public PhaseNameSet(string fullName, string keyName, List<string> otherNames) : this()
        {
            FullName = fullName;
            KeyName = keyName;
            OtherNames = otherNames;
            // BuildNameCache();
        }

        
        // 1. Create a HashSet field to store all possible names
        // private HashSet<string> _allNamesCache;

        // 2. Add a method to build the cache. 
        // (Call this inside the PhaseNameSet constructor, or right after its data is loaded)
        // public void BuildNameCache()
        // {
        //     // OrdinalIgnoreCase makes hash lookups lightning fast
        //     _allNamesCache = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        //     if (!string.IsNullOrWhiteSpace(FullName)) _allNamesCache.Add(FullName);
        //     if (!string.IsNullOrWhiteSpace(KeyName)) _allNamesCache.Add(KeyName);
        //
        //     if (OtherNames != null)
        //     {
        //         foreach(var otherName in OtherNames)
        //             if (!string.IsNullOrWhiteSpace(otherName)) _allNamesCache.Add(otherName);
        //     }
        // }
        //
        // 3. Update HasName to use the cache
        // public bool HasName(string name)
        // {
        //     if (string.IsNullOrWhiteSpace(name) || _allNamesCache == null) return false;
        //
        //     // This is now an O(1) instantaneous lookup
        //     return _allNamesCache.Contains(name);
        // }
        
        public bool HasName(string name)
        {
            return FullName.Equals(name, StringComparison.OrdinalIgnoreCase)
                || KeyName.Equals(name, StringComparison.Ordinal)
                || OtherNames.Contains(name, StringComparer.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(FullName) ? KeyName : FullName;
        }

        public PhaseNameSet Copy(bool withId)
        {
            var copy = new PhaseNameSet();
            copy.Id = withId ? Id : Guid.NewGuid();
            copy.FullName = FullName;
            copy.KeyName = KeyName;
            copy.OtherNames = OtherNames;
            return copy;
        }
    }
}

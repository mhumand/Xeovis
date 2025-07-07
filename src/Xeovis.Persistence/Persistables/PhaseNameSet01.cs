using Xeovis.Domain.Definitions.Experiment;

namespace Xeovis.Persistence
{
    public class PhaseNameSet01
    {
        public string FullName { get; set; }
        public string KeyName { get; set; }
        public List<string> OtherNames { get; set; }
        public Guid Id { get; set; }

        public PhaseNameSet01()
        {
            
        }

        public PhaseNameSet01(PhaseNameSet realObject)
        {
            FullName = realObject.FullName;
            KeyName = realObject.KeyName;
            OtherNames = realObject.OtherNames;
            Id = realObject.Id;
        }

        public PhaseNameSet ConvertToRealObject()
        {
            return new PhaseNameSet()
            {
                FullName = FullName,
                KeyName = KeyName,
                OtherNames = OtherNames,
                Id = Id
            };
        }
    }
}

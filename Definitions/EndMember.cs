using DatabaseReader.Enums;

namespace DatabaseReader.Definitions
{
    public class EndMember
    {
        public string Name { get; set; }
        public EndMemberType Type { get; set; }
        public Phase OwnerPhase { get; set; }
        public string Formula { get; set; }
        public bool IsOrderEndMember { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

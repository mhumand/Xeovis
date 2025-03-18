namespace Xeovis.Domain
{
    public class EndMember
    {
        public string Name { get; set; }
        public EndMemberType Type { get; set; }
        public Phase Phase { get; set; }
        public string Formula { get; set; }
        public bool IsOrderEndMember { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

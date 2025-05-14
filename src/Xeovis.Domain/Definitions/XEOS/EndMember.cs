namespace Xeovis.Domain
{
    public class EndMember
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public EndMemberType Type { get; set; }
        public Guid PhaseId { get; set; }
        public string Formula { get; set; }
        public bool IsOrderEndMember { get; set; }

        public EndMember()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Formula = string.Empty;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

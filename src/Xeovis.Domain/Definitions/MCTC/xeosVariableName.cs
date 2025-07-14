namespace Xeovis.Domain.Definitions.MCTC
{
    public class xeosVariableName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PhaseId { get; set; }
        public bool IsOrderVariable { get; set; }

        public xeosVariableName()
        {
            Name = string.Empty;
            Id = Guid.NewGuid();
            PhaseId = Guid.Empty;
        }

        public xeosVariableName(string name) : this()
        {
            Name = name;
        }

        public xeosVariableName(string name, Guid phaseId) : this()
        {
            Name = name;
            PhaseId = phaseId;
        }

        public xeosVariableName Copy(bool withId)
        {
            return new xeosVariableName()
            {
                Id = withId ? Id : Guid.NewGuid(),
                Name = Name,
                PhaseId = PhaseId
            };
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

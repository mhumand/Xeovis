using Xeovis.Domain.Definitions.MCTC;

namespace Xeovis.Persistence.Persistables
{
    public class xeosVariableName01
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PhaseId { get; set; }
        public bool IsOrderVariable { get; set; }

        public xeosVariableName01()
        {
            
        }

        public xeosVariableName01(xeosVariableName realObject)
        {
            Id = realObject.Id;
            Name = realObject.Name;
            PhaseId = realObject.PhaseId;
            IsOrderVariable = realObject.IsOrderVariable;
        }

        public xeosVariableName ConvertToRealObject()
        {
            return new xeosVariableName()
            {
                Id = Id,
                Name = Name,
                PhaseId = PhaseId,
                IsOrderVariable = IsOrderVariable
            };
        }
    }
}

using Xeovis.Domain;

namespace Xeovis.Persistence.Proxies;

public class Phase01
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public PhaseType Type { get; set; }
    public PhaseState State { get; set; }
    public List<string> OtherNames { get; set; }
    public List<EndMember> EndMembers { get; set; }
    public List<OxideType> Oxides { get; set; }
    public List<double> Composition { get; set; }
    public Guid ExpRunId { get; set; }
    public string Comment { get; set; }

    public Phase01()
    {
        
    }
    
    public Phase01(Phase realObject)
    {
        Name = realObject.Name;
        Id = realObject.Id;
        Type = realObject.Type;
        State = realObject.State;
        OtherNames = new List<string>(realObject.OtherNames);
        EndMembers = new List<EndMember>(realObject.EndMembers);
        Oxides = new List<OxideType>(realObject.Oxides);
        Composition = new List<double>(realObject.Composition);
        ExpRunId = realObject.ExpRunId;
        Comment = realObject.Comment;
    }

    public Phase ConvertToRealObject()
    {
        return new Phase()
        {
            Name = Name,
            Id = Id,
            Type = Type,
            State = State,
            OtherNames = [..OtherNames],
            EndMembers = [..EndMembers],
            Oxides = [..Oxides],
            Composition = [..Composition],
            ExpRunId = ExpRunId,
            Comment = Comment
        };
    }
}
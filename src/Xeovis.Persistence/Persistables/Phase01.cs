using Xeovis.Domain;

namespace Xeovis.Persistence.Proxies;

public class Phase01
{
    public Guid NameId { get; set; }
    public Guid Id { get; set; }
    public PhaseState State { get; set; }
    public List<EndMember> EndMembers { get; set; }
    public Dictionary<OxideType, double> Composition { get; set; }
    public Guid ExpRunId { get; set; }
    public string Comment { get; set; }

    public Phase01()
    {
    }

    public Phase01(Phase realObject)
    {
        NameId = realObject.NameId;
        Id = realObject.Id;
        State = realObject.State;
        EndMembers = new List<EndMember>(realObject.EndMembers);
        Composition = new Dictionary<OxideType, double>(realObject.Composition);
        ExpRunId = realObject.ExpRunId;
        Comment = realObject.Comment;
    }

    public Phase ConvertToRealObject()
    {
        return new Phase()
        {
            NameId = NameId,
            Id = Id,
            State = State,
            EndMembers = [..EndMembers],
            Composition = new Dictionary<OxideType, double>(Composition),
            ExpRunId = ExpRunId,
            Comment = Comment
        };
    }
}
namespace Xeovis.Domain.Tests.xeos.Tests;

public class xeosTests
{
    private XEOS xeos;

    [SetUp]
    public void Setup()
    {
        xeos = new XEOS();
        xeos.EndMembers = new List<EndMember>()
        {
            new EndMember() { Name = "endmember1" },
            new EndMember() { Name = "endmember2" },
            new EndMember() { Name = "endmember3" },
        };

        xeos.Ws = new List<EnergyParameter>()
        {
            new EnergyParameter()
            {
                A = 0.5,
                Endmember1 = xeos.EndMembers[0],
                Endmember2 = xeos.EndMembers[1]
            },
            new EnergyParameter()
            {
                A = 22,
                Endmember1 = xeos.EndMembers[2],
                Endmember2 = xeos.EndMembers[1]
            },
            new EnergyParameter()
            {
                A = 1,
                Endmember1 = xeos.EndMembers[2],
                Endmember2 = xeos.EndMembers[0]
            },
        };
    }

    [Test]
    public void GetWij()
    {
        Assert.That(xeos.GetWij(xeos.EndMembers[0], xeos.EndMembers[1]), Is.EqualTo(xeos.Ws[0]));
        Assert.That(xeos.GetWij(xeos.EndMembers[1], xeos.EndMembers[0]), Is.EqualTo(xeos.Ws[0]));
        Assert.That(xeos.GetWij(xeos.EndMembers[2], xeos.EndMembers[1]), Is.EqualTo(xeos.Ws[1]));
        Assert.That(xeos.GetWij(xeos.EndMembers[1], xeos.EndMembers[2]), Is.EqualTo(xeos.Ws[1]));
        Assert.That(xeos.GetWij(xeos.EndMembers[0], xeos.EndMembers[2]), Is.EqualTo(xeos.Ws[2]));
        Assert.That(xeos.GetWij(xeos.EndMembers[2], xeos.EndMembers[0]), Is.EqualTo(xeos.Ws[2]));
    }
}
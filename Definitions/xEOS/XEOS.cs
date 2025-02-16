using DatabaseReader.Enums;
using System.Collections.Generic;

namespace DatabaseReader.Definitions.xEOS
{
    public class XEOS
    {
        public string Name { get; set; }
        public PhaseType Phase { get; set; }
        public List<EndMember> EndMembers { get; set; }
        public List<EnergyParameter> Ws { get; set; }


        public XEOS()
        {
            EndMembers = new List<EndMember>();
            Ws = new List<EnergyParameter>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

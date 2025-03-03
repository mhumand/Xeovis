using DatabaseReader.Enums;
using System.Collections.Generic;

namespace DatabaseReader.Definitions.xEOS
{
    public class XEOS
    {
        public string Name { get; set; }
        public PhaseType PhaseType { get; set; }
        public List<EndMember> EndMembers { get; set; }
        public List<EnergyParameter> Ws { get; set; }
        public List<AlphaParameter> Alphas { get; set; }
        public List<ModelVariable> Variables { get; set; }
        public XEOS()
        {
            EndMembers = new List<EndMember>();
            Ws = new List<EnergyParameter>();
            Alphas = new List<AlphaParameter>();
            Variables = new List<ModelVariable>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

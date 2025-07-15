using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Domain.Definitions.MCTC;

namespace Xeovis.Domain
{
    public static class DefaultXeosVariableCations
    {
        public static Dictionary<Guid, List<CationType>> GetDefaultXeosVariableCations()
        {
            var variableCations = new Dictionary<Guid, List<CationType>>();
            var phaseList = PhaseNameList.PhaseNames;
            foreach ( var phase in phaseList )
            {
                var res = xeosVariableNameList.xeosVariableNames.TryGetValue(phase.Id, out List<xeosVariableName>? variables);
                if (!res || variables == null) continue;
                foreach (var variable in variables)
                {
                    var cations = GetCationsFromModelVariable(phase.FullName, variable.Name);
                    if (cations == null || cations.Count == 0) continue;
                    variableCations.Add(variable.Id, cations);
                }
            }

            return variableCations;
        }

        private static List<CationType> GetCationsFromModelVariable(string phaseName, string variableName)
        {
            var nullSet = new PhaseNameSet();

            if (PhaseNameList.GetNameSetByString("garnet", nullSet).HasName(phaseName))
                return GetVariableCations_Garnet(variableName);

            if (PhaseNameList.GetNameSetByString("olivine", nullSet).HasName(phaseName))
                return GetVariableCations_Olivine(variableName);

            if (PhaseNameList.GetNameSetByString("ilmenite", nullSet).HasName(phaseName))
                return GetVariableCations_Ilmenite(variableName);

            if (PhaseNameList.GetNameSetByString("clinopyroxene", nullSet).HasName(phaseName))
                return GetVariableCations_Cpx(variableName);

            if (PhaseNameList.GetNameSetByString("orthopyroxene", nullSet).HasName(phaseName))
                return GetVariableCations_Opx(variableName);

            if (PhaseNameList.GetNameSetByString("spinel", nullSet).HasName(phaseName))
                return GetVariableCations_Spinel(variableName);

            if (PhaseNameList.GetNameSetByString("epidote", nullSet).HasName(phaseName))
                return GetVariableCations_Epidote(variableName);

            if (PhaseNameList.GetNameSetByString("plagioclase", nullSet).HasName(phaseName))
                return GetVariableCations_Plagioclase(variableName);

            if (PhaseNameList.GetNameSetByString("biotite", nullSet).HasName(phaseName))
                return GetVariableCations_Biotite(variableName);

            return new List<CationType>();
        }

        private static List<CationType> GetVariableCations_Garnet(string variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case "x":
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case "c":
                    list.Add(CationType.Ca);
                    break;
                case "f":
                    list.Add(CationType.Fe3);
                    break;
                case "cr":
                    list.Add(CationType.Cr);
                    break;
                case "t":
                    list.Add(CationType.Ti);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Olivine(string variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case "x":
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case "c":
                    list.Add(CationType.Ca);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Ilmenite(string variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case "i":
                    list.Add(CationType.Fe3);
                    break;
                case "m":
                    list.Add(CationType.Mg);
                    list.Add(CationType.Fe2);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Cpx(string variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case "x":
                case "o":
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case "y":
                    list.Add(CationType.Al);
                    break;
                case "n":
                    list.Add(CationType.Na);
                    break;
                case "f":
                    list.Add(CationType.Fe3);
                    break;
                case "cr":
                    list.Add(CationType.Cr);
                    break;
                case "t":
                    list.Add(CationType.Ti);
                    break;
                case "k":
                    list.Add(CationType.K);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Opx(string variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case "x":
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case "y":
                    list.Add(CationType.Al);
                    break;
                case "c":
                    list.Add(CationType.Ca);
                    break;
                case "f":
                    list.Add(CationType.Fe3);
                    break;
                case "cr":
                    list.Add(CationType.Cr);
                    break;
                case "t":
                    list.Add(CationType.Ti);
                    break;
                case "j":
                    list.Add(CationType.Na);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Spinel(string variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case "x":
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case "y":
                    list.Add(CationType.Fe3);
                    list.Add(CationType.Al);
                    break;
                case "c":
                    list.Add(CationType.Cr);
                    break;
                case "t":
                    list.Add(CationType.Ti);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Epidote(string variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case "f":
                    list.Add(CationType.Fe3);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Biotite(string variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case "x":
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case "y":
                    list.Add(CationType.Al);
                    break;
                case "f":
                    list.Add(CationType.Fe3);
                    break;
                case "t":
                    list.Add(CationType.Ti);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Plagioclase(string variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case "ca":
                    list.Add(CationType.Ca);
                    break;
                case "k":
                    list.Add(CationType.K);
                    break;
            }

            return list;
        }
    }
}
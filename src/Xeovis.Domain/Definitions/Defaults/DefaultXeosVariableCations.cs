using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Domain.Definitions.MCTC;

namespace Xeovis.Domain
{
    public static class DefaultXeosVariableCations
    {
        public static Dictionary<Guid, List<Element>> GetDefaultXeosVariableCations()
        {
            var variableCations = new Dictionary<Guid, List<Element>>();
            var phaseList = PhaseNameList.PhaseNames;
            foreach ( var phase in phaseList )
            {
                var res = xeosVariableNameList.VariableNames.TryGetValue(phase.Id, out List<xeosVariableName>? variables);
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

        private static List<Element> GetCationsFromModelVariable(string phaseName, string variableName)
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

            return new List<Element>();
        }

        private static List<Element> GetVariableCations_Garnet(string variable)
        {
            var list = new List<Element>();
            switch (variable)
            {
                case "x":
                    list.Add(Element.Fe2);
                    list.Add(Element.Mg);
                    break;
                case "c":
                    list.Add(Element.Ca);
                    break;
                case "f":
                    list.Add(Element.Fe3);
                    break;
                case "cr":
                    list.Add(Element.Cr);
                    break;
                case "t":
                    list.Add(Element.Ti);
                    break;
            }

            return list;
        }

        private static List<Element> GetVariableCations_Olivine(string variable)
        {
            var list = new List<Element>();
            switch (variable)
            {
                case "x":
                    list.Add(Element.Fe2);
                    list.Add(Element.Mg);
                    break;
                case "c":
                    list.Add(Element.Ca);
                    break;
            }

            return list;
        }

        private static List<Element> GetVariableCations_Ilmenite(string variable)
        {
            var list = new List<Element>();
            switch (variable)
            {
                case "i":
                    list.Add(Element.Fe3);
                    break;
                case "m":
                    list.Add(Element.Mg);
                    list.Add(Element.Fe2);
                    break;
            }

            return list;
        }

        private static List<Element> GetVariableCations_Cpx(string variable)
        {
            var list = new List<Element>();
            switch (variable)
            {
                case "x":
                case "o":
                    list.Add(Element.Fe2);
                    list.Add(Element.Mg);
                    break;
                case "y":
                    list.Add(Element.Al);
                    break;
                case "n":
                    list.Add(Element.Na);
                    break;
                case "f":
                    list.Add(Element.Fe3);
                    break;
                case "cr":
                    list.Add(Element.Cr);
                    break;
                case "t":
                    list.Add(Element.Ti);
                    break;
                case "k":
                    list.Add(Element.K);
                    break;
            }

            return list;
        }

        private static List<Element> GetVariableCations_Opx(string variable)
        {
            var list = new List<Element>();
            switch (variable)
            {
                case "x":
                    list.Add(Element.Fe2);
                    list.Add(Element.Mg);
                    break;
                case "y":
                    list.Add(Element.Al);
                    break;
                case "c":
                    list.Add(Element.Ca);
                    break;
                case "f":
                    list.Add(Element.Fe3);
                    break;
                case "cr":
                    list.Add(Element.Cr);
                    break;
                case "t":
                    list.Add(Element.Ti);
                    break;
                case "j":
                    list.Add(Element.Na);
                    break;
            }

            return list;
        }

        private static List<Element> GetVariableCations_Spinel(string variable)
        {
            var list = new List<Element>();
            switch (variable)
            {
                case "x":
                    list.Add(Element.Fe2);
                    list.Add(Element.Mg);
                    break;
                case "y":
                    list.Add(Element.Fe3);
                    list.Add(Element.Al);
                    break;
                case "c":
                    list.Add(Element.Cr);
                    break;
                case "t":
                    list.Add(Element.Ti);
                    break;
            }

            return list;
        }

        private static List<Element> GetVariableCations_Epidote(string variable)
        {
            var list = new List<Element>();
            switch (variable)
            {
                case "f":
                    list.Add(Element.Fe3);
                    break;
            }

            return list;
        }

        private static List<Element> GetVariableCations_Biotite(string variable)
        {
            var list = new List<Element>();
            switch (variable)
            {
                case "x":
                    list.Add(Element.Fe2);
                    list.Add(Element.Mg);
                    break;
                case "y":
                    list.Add(Element.Al);
                    break;
                case "f":
                    list.Add(Element.Fe3);
                    break;
                case "t":
                    list.Add(Element.Ti);
                    break;
            }

            return list;
        }

        private static List<Element> GetVariableCations_Plagioclase(string variable)
        {
            var list = new List<Element>();
            switch (variable)
            {
                case "ca":
                    list.Add(Element.Ca);
                    break;
                case "k":
                    list.Add(Element.K);
                    break;
            }

            return list;
        }
    }
}
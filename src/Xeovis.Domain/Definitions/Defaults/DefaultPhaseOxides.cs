using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;

namespace Xeovis.Domain
{
    public static class DefaultPhaseOxides
    {
        public static Dictionary<Guid, List<OxideType>> GetDefaultPhaseOxides()
        {
            var phaseOxides = new Dictionary<Guid, List<OxideType>>();
            var phaseNames = PhaseNameList.PhaseNames;
            foreach (var phaseName in phaseNames)
            {
                var oxides = GetPhaseOxides(phaseName.FullName);
                if (oxides == null || oxides.Count == 0) continue;
                phaseOxides.Add(phaseName.Id, oxides);
            }

            return phaseOxides;
        }

        public static List<OxideType> GetPhaseOxides(string phaseName)
        {
            var nullSet = new PhaseNameSet();
            // pure phases
            if (PhaseNameList.GetNameSetByString("quartz", nullSet).HasName(phaseName)
                || PhaseNameList.GetNameSetByString("coesite", nullSet).HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2 };

            if (PhaseNameList.GetNameSetByString("zoisite", nullSet).HasName(phaseName))
                return new List<OxideType>() { OxideType.H2O, OxideType.SiO2, OxideType.Al2O3, OxideType.CaO };

            if (PhaseNameList.GetNameSetByString("kyanite", nullSet).HasName(phaseName)
                || PhaseNameList.GetNameSetByString("andalusite", nullSet).HasName(phaseName)
                || PhaseNameList.GetNameSetByString("sillimanite", nullSet).HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2, OxideType.Al2O3 };

            if (PhaseNameList.GetNameSetByString("rutile", nullSet).HasName(phaseName))
                return new List<OxideType>() { OxideType.TiO2 };

            if (PhaseNameList.GetNameSetByString("sphene", nullSet).HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2, OxideType.TiO2, OxideType.CaO };

            if (PhaseNameList.GetNameSetByString("water", nullSet).HasName(phaseName))
                return new List<OxideType>() { OxideType.H2O };

            // solid solutions
            if (PhaseNameList.GetNameSetByString("garnet", nullSet).HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.MgO, OxideType.FeO, OxideType.CaO, OxideType.Al2O3, OxideType.Fe2O3,
                        OxideType.Cr2O3, OxideType.TiO2
                    };

            if (PhaseNameList.GetNameSetByString("olivine", nullSet).HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2, OxideType.MgO, OxideType.CaO, OxideType.FeO };

            if (PhaseNameList.GetNameSetByString("ilmenite", nullSet).HasName(phaseName))
                return new List<OxideType>() { OxideType.FeO, OxideType.MgO, OxideType.TiO2, OxideType.Fe2O3 };

            if (PhaseNameList.GetNameSetByString("clinopyroxene", nullSet).HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.Na2O, OxideType.K2O, OxideType.CaO, OxideType.MgO, OxideType.FeO,
                        OxideType.Al2O3, OxideType.Fe2O3, OxideType.TiO2, OxideType.Cr2O3
                    };

            if (PhaseNameList.GetNameSetByString("orthopyroxene", nullSet).HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.Na2O, OxideType.MgO, OxideType.CaO, OxideType.FeO, OxideType.Al2O3,
                        OxideType.Fe2O3, OxideType.TiO2, OxideType.Cr2O3
                    };

            if (PhaseNameList.GetNameSetByString("spinel", nullSet).HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.MgO, OxideType.FeO, OxideType.Al2O3, OxideType.Fe2O3, OxideType.Cr2O3, OxideType.TiO2
                    };

            if (PhaseNameList.GetNameSetByString("epidote", nullSet).HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2, OxideType.CaO, OxideType.H2O, OxideType.Fe2O3, OxideType.Al2O3 };

            if (PhaseNameList.GetNameSetByString("biotite", nullSet).HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.K2O, OxideType.FeO, OxideType.MgO, OxideType.TiO2, OxideType.Fe2O3,
                        OxideType.Al2O3, OxideType.H2O
                    };

            if (PhaseNameList.GetNameSetByString("plagioclase", nullSet).HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.CaO, OxideType.Na2O, OxideType.K2O, OxideType.Al2O3
                    };

            return null;
        }
    }
}
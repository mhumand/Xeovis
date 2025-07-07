using Xeovis.Domain.Definitions;

namespace Xeovis.Domain
{
    public static class DefaultPhaseCompositions
    {
        public static List<OxideType> GetOxidesFromPhaseComposition(string phaseName)
        {
            // pure phases
            if (PhaseNameList.GetNameSetByString("q").HasName(phaseName)
                || PhaseNameList.GetNameSetByString("coe").HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2 };

            if (PhaseNameList.GetNameSetByString("zo").HasName(phaseName))
                return new List<OxideType>() { OxideType.H2O, OxideType.SiO2, OxideType.Al2O3, OxideType.CaO };

            if (PhaseNameList.GetNameSetByString("ky").HasName(phaseName)
                || PhaseNameList.GetNameSetByString("and").HasName(phaseName)
                || PhaseNameList.GetNameSetByString("sill").HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2, OxideType.Al2O3 };

            if (PhaseNameList.GetNameSetByString("ru").HasName(phaseName))
                return new List<OxideType>() { OxideType.TiO2 };

            if (PhaseNameList.GetNameSetByString("sph").HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2, OxideType.TiO2, OxideType.CaO };

            if (PhaseNameList.GetNameSetByString("h2o").HasName(phaseName))
                return new List<OxideType>() { OxideType.H2O };

            // solid solutions
            if (PhaseNameList.GetNameSetByString("gt").HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.MgO, OxideType.FeO, OxideType.CaO, OxideType.Al2O3, OxideType.Fe2O3,
                        OxideType.Cr2O3, OxideType.TiO2
                    };

            if (PhaseNameList.GetNameSetByString("ol").HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2, OxideType.MgO, OxideType.CaO, OxideType.FeO };

            if (PhaseNameList.GetNameSetByString("ilm").HasName(phaseName))
                return new List<OxideType>() { OxideType.FeO, OxideType.MgO, OxideType.TiO2, OxideType.Fe2O3 };

            if (PhaseNameList.GetNameSetByString("cpx").HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.Na2O, OxideType.K2O, OxideType.CaO, OxideType.MgO, OxideType.FeO,
                        OxideType.Al2O3, OxideType.Fe2O3, OxideType.TiO2, OxideType.Cr2O3
                    };

            if (PhaseNameList.GetNameSetByString("opx").HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.Na2O, OxideType.MgO, OxideType.CaO, OxideType.FeO, OxideType.Al2O3,
                        OxideType.Fe2O3, OxideType.TiO2, OxideType.Cr2O3
                    };

            if (PhaseNameList.GetNameSetByString("spn").HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.MgO, OxideType.FeO, OxideType.Al2O3, OxideType.Fe2O3, OxideType.Cr2O3, OxideType.TiO2
                    };

            if (PhaseNameList.GetNameSetByString("ep").HasName(phaseName))
                return new List<OxideType>() { OxideType.SiO2, OxideType.CaO, OxideType.H2O, OxideType.Fe2O3, OxideType.Al2O3 };

            if (PhaseNameList.GetNameSetByString("bi").HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.K2O, OxideType.FeO, OxideType.MgO, OxideType.TiO2, OxideType.Fe2O3,
                        OxideType.Al2O3, OxideType.H2O
                    };

            if (PhaseNameList.GetNameSetByString("pl").HasName(phaseName))
                return new List<OxideType>()
                    {
                        OxideType.SiO2, OxideType.CaO, OxideType.Na2O, OxideType.K2O, OxideType.Al2O3
                    };

            return null;
        }

        public static List<CationType> GetCationsFromPhaseComposition(string phaseType)
        {
            var oxides = GetOxidesFromPhaseComposition(phaseType);
            var cations = new List<CationType>();
            if (oxides == null) return cations;
            foreach (var oxide in oxides)
                cations.Add(oxide.GetCationType());

            return cations;
        }
    }
}
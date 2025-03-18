using System.Collections.Generic;

namespace Xeovis.Domain
{
    public static class DefaultPhaseCompositions
    {
        public static List<OxideType> GetOxidesFromPhaseComposition(PhaseType phaseType)
        {
            // pure phases
            if (phaseType == PhaseType.q || phaseType == PhaseType.coe)
                return new List<OxideType>() { OxideType.SiO2 };

            if (phaseType == PhaseType.zo)
                return new List<OxideType>() { OxideType.H2O, OxideType.SiO2, OxideType.Al2O3, OxideType.CaO };

            if (phaseType == PhaseType.ky || phaseType == PhaseType.and || phaseType == PhaseType.sill)
                return new List<OxideType>() { OxideType.SiO2, OxideType.Al2O3 };

            if (phaseType == PhaseType.ru)
                return new List<OxideType>() { OxideType.TiO2 };

            if (phaseType == PhaseType.sph)
                return new List<OxideType>() { OxideType.SiO2, OxideType.TiO2, OxideType.CaO };

            if (phaseType == PhaseType.H2O)
                return new List<OxideType>() { OxideType.H2O };

            // solid solutions
            if (phaseType == PhaseType.g)
                return new List<OxideType>() { OxideType.SiO2, OxideType.MgO, OxideType.FeO, OxideType.CaO, OxideType.Al2O3, OxideType.Fe2O3, OxideType.Cr2O3, OxideType.TiO2 };

            if (phaseType == PhaseType.ol)
                return new List<OxideType>() { OxideType.SiO2, OxideType.MgO, OxideType.CaO, OxideType.FeO };

            if (phaseType == PhaseType.ilm)
                return new List<OxideType>() { OxideType.FeO, OxideType.MgO, OxideType.TiO2, OxideType.Fe2O3 };

            if (phaseType == PhaseType.cpx)
                return new List<OxideType>() { OxideType.SiO2, OxideType.Na2O, OxideType.K2O, OxideType.CaO, OxideType.MgO, OxideType.FeO, OxideType.Al2O3, OxideType.Fe2O3, OxideType.TiO2, OxideType.Cr2O3 };

            if (phaseType == PhaseType.opx)
                return new List<OxideType>() { OxideType.SiO2, OxideType.Na2O, OxideType.MgO, OxideType.CaO, OxideType.FeO, OxideType.Al2O3, OxideType.Fe2O3, OxideType.TiO2, OxideType.Cr2O3 };

            if (phaseType == PhaseType.spn)
                return new List<OxideType>() { OxideType.MgO, OxideType.FeO, OxideType.Al2O3, OxideType.Fe2O3, OxideType.Cr2O3, OxideType.TiO2 };

            if (phaseType == PhaseType.ep)
                return new List<OxideType>() { OxideType.SiO2, OxideType.CaO, OxideType.H2O, OxideType.Fe2O3, OxideType.Al2O3 };

            if (phaseType == PhaseType.bi)
                return new List<OxideType>() { OxideType.SiO2, OxideType.K2O, OxideType.FeO, OxideType.MgO, OxideType.TiO2, OxideType.Fe2O3, OxideType.Al2O3, OxideType.H2O };

            if (phaseType == PhaseType.pl)
                return new List<OxideType>() { OxideType.SiO2, OxideType.CaO, OxideType.Na2O, OxideType.K2O, OxideType.Al2O3 };

            return null;
        }

        public static List<CationType> GetCationsFromPhaseComposition(PhaseType phaseType)
        {
            var oxides = GetOxidesFromPhaseComposition(phaseType);

            var cations = new List<CationType>();
            foreach (var oxide in oxides)
                cations.Add(oxide.GetCationType());

            return cations;
        }
    }
}

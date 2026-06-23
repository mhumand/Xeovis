using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;

namespace Xeovis.Domain
{
    public static class DefaultPhaseComponents
    {
        public static Dictionary<Guid, List<Component>> GetDefaultPhaseComponents()
        {
            var phaseComponents = new Dictionary<Guid, List<Component>>();
            var phaseNames = PhaseNameList.PhaseNames;
            foreach (var phaseName in phaseNames)
            {
                var components = GetPhaseComponents(phaseName.FullName);
                if (components == null || components.Count == 0) continue;
                phaseComponents.Add(phaseName.Id, components);
            }

            return phaseComponents;
        }

        public static List<Component> GetPhaseComponents(string phaseName)
        {
            var nullSet = new PhaseNameSet();
            // pure phases
            if (PhaseNameList.GetNameSetByString("quartz", nullSet).HasName(phaseName)
                || PhaseNameList.GetNameSetByString("coesite", nullSet).HasName(phaseName))
                return new List<Component>() { Component.SiO2 };

            if (PhaseNameList.GetNameSetByString("zoisite", nullSet).HasName(phaseName))
                return new List<Component>() { Component.H2O, Component.SiO2, Component.Al2O3, Component.CaO };

            if (PhaseNameList.GetNameSetByString("kyanite", nullSet).HasName(phaseName)
                || PhaseNameList.GetNameSetByString("andalusite", nullSet).HasName(phaseName)
                || PhaseNameList.GetNameSetByString("sillimanite", nullSet).HasName(phaseName))
                return new List<Component>() { Component.SiO2, Component.Al2O3 };

            if (PhaseNameList.GetNameSetByString("rutile", nullSet).HasName(phaseName))
                return new List<Component>() { Component.TiO2 };

            if (PhaseNameList.GetNameSetByString("sphene", nullSet).HasName(phaseName))
                return new List<Component>() { Component.SiO2, Component.TiO2, Component.CaO };

            if (PhaseNameList.GetNameSetByString("water", nullSet).HasName(phaseName))
                return new List<Component>() { Component.H2O };

            // solid solutions
            if (PhaseNameList.GetNameSetByString("garnet", nullSet).HasName(phaseName))
                return new List<Component>()
                    {
                        Component.SiO2, Component.MgO, Component.FeO, Component.CaO, Component.Al2O3, Component.Fe2O3,
                        Component.Cr2O3, Component.TiO2
                    };

            if (PhaseNameList.GetNameSetByString("olivine", nullSet).HasName(phaseName))
                return new List<Component>() { Component.SiO2, Component.MgO, Component.CaO, Component.FeO };

            if (PhaseNameList.GetNameSetByString("ilmenite", nullSet).HasName(phaseName))
                return new List<Component>() { Component.FeO, Component.MgO, Component.TiO2, Component.Fe2O3 };

            if (PhaseNameList.GetNameSetByString("clinopyroxene", nullSet).HasName(phaseName))
                return new List<Component>()
                    {
                        Component.SiO2, Component.Na2O, Component.K2O, Component.CaO, Component.MgO, Component.FeO,
                        Component.Al2O3, Component.Fe2O3, Component.TiO2, Component.Cr2O3
                    };

            if (PhaseNameList.GetNameSetByString("orthopyroxene", nullSet).HasName(phaseName))
                return new List<Component>()
                    {
                        Component.SiO2, Component.Na2O, Component.MgO, Component.CaO, Component.FeO, Component.Al2O3,
                        Component.Fe2O3, Component.TiO2, Component.Cr2O3
                    };

            if (PhaseNameList.GetNameSetByString("spinel", nullSet).HasName(phaseName))
                return new List<Component>()
                    {
                        Component.MgO, Component.FeO, Component.Al2O3, Component.Fe2O3, Component.Cr2O3, Component.TiO2
                    };

            if (PhaseNameList.GetNameSetByString("epidote", nullSet).HasName(phaseName))
                return new List<Component>() { Component.SiO2, Component.CaO, Component.H2O, Component.Fe2O3, Component.Al2O3 };

            if (PhaseNameList.GetNameSetByString("biotite", nullSet).HasName(phaseName))
                return new List<Component>()
                    {
                        Component.SiO2, Component.K2O, Component.FeO, Component.MgO, Component.TiO2, Component.Fe2O3,
                        Component.Al2O3, Component.H2O
                    };

            if (PhaseNameList.GetNameSetByString("plagioclase", nullSet).HasName(phaseName))
                return new List<Component>()
                    {
                        Component.SiO2, Component.CaO, Component.Na2O, Component.K2O, Component.Al2O3
                    };

            return null;
        }
    }
}
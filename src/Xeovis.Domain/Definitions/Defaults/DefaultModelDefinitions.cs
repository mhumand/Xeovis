using Xeovis.Domain.Definitions;

namespace Xeovis.Domain
{
    public static class DefaultModelDefinitions
    {
        public static List<CationType> GetCationsFromModelVariable(string phase, ModelVariableType variable)
        {
            if (PhaseNameList.GetNameSetByString("g").HasName(phase))
                return GetVariableCations_Garnet(variable);

            if (PhaseNameList.GetNameSetByString("ol").HasName(phase))
                return GetVariableCations_Olivine(variable);

            if (PhaseNameList.GetNameSetByString("ilm").HasName(phase))
                return GetVariableCations_Ilmenite(variable);

            if (PhaseNameList.GetNameSetByString("cpx").HasName(phase))
                return GetVariableCations_Cpx(variable);

            if (PhaseNameList.GetNameSetByString("opx").HasName(phase))
                return GetVariableCations_Opx(variable);

            if (PhaseNameList.GetNameSetByString("spn").HasName(phase))
                return GetVariableCations_Spinel(variable);

            if (PhaseNameList.GetNameSetByString("ep").HasName(phase))
                return GetVariableCations_Epidote(variable);

            if (PhaseNameList.GetNameSetByString("pl").HasName(phase))
                return GetVariableCations_Plagioclase(variable);

            if (PhaseNameList.GetNameSetByString("bi").HasName(phase))
                return GetVariableCations_Biotite(variable);

            if (PhaseNameList.GetNameSetByString("phe").HasName(phase))
                return GetVariableCations_Phengite(variable);

            return new List<CationType>();
        }

        private static List<CationType> GetVariableCations_Garnet(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.x:
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case ModelVariableType.c:
                    list.Add(CationType.Ca);
                    break;
                case ModelVariableType.f:
                    list.Add(CationType.Fe3);
                    break;
                case ModelVariableType.cr:
                    list.Add(CationType.Cr);
                    break;
                case ModelVariableType.t:
                    list.Add(CationType.Ti);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Olivine(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.x:
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case ModelVariableType.c:
                    list.Add(CationType.Ca);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Ilmenite(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.i:
                    list.Add(CationType.Fe3);
                    break;
                case ModelVariableType.m:
                    list.Add(CationType.Mg);
                    list.Add(CationType.Fe2);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Cpx(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.x:
                case ModelVariableType.o:
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case ModelVariableType.y:
                    list.Add(CationType.Al);
                    break;
                case ModelVariableType.n:
                    list.Add(CationType.Na);
                    break;
                case ModelVariableType.f:
                    list.Add(CationType.Fe3);
                    break;
                case ModelVariableType.cr:
                    list.Add(CationType.Cr);
                    break;
                case ModelVariableType.t:
                    list.Add(CationType.Ti);
                    break;
                case ModelVariableType.k:
                    list.Add(CationType.K);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Opx(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.x:
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case ModelVariableType.y:
                    list.Add(CationType.Al);
                    break;
                case ModelVariableType.c:
                    list.Add(CationType.Ca);
                    break;
                case ModelVariableType.f:
                    list.Add(CationType.Fe3);
                    break;
                case ModelVariableType.cr:
                    list.Add(CationType.Cr);
                    break;
                case ModelVariableType.t:
                    list.Add(CationType.Ti);
                    break;
                case ModelVariableType.j:
                    list.Add(CationType.Na);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Spinel(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.x:
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case ModelVariableType.y:
                    list.Add(CationType.Fe3);
                    list.Add(CationType.Al);
                    break;
                case ModelVariableType.c:
                    list.Add(CationType.Cr);
                    break;
                case ModelVariableType.t:
                    list.Add(CationType.Ti);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Epidote(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.f:
                    list.Add(CationType.Fe3);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Biotite(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.x:
                    list.Add(CationType.Fe2);
                    list.Add(CationType.Mg);
                    break;
                case ModelVariableType.y:
                    list.Add(CationType.Al);
                    break;
                case ModelVariableType.f:
                    list.Add(CationType.Fe3);
                    break;
                case ModelVariableType.t:
                    list.Add(CationType.Ti);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Plagioclase(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.ca:
                    list.Add(CationType.Ca);
                    break;
                case ModelVariableType.k:
                    list.Add(CationType.K);
                    break;
            }

            return list;
        }

        private static List<CationType> GetVariableCations_Phengite(ModelVariableType variable)
        {
            var list = new List<CationType>();
            switch (variable)
            {
                case ModelVariableType.x:
                    break;
                case ModelVariableType.y:
                    break;
                case ModelVariableType.c:
                    break;
                case ModelVariableType.o:
                    break;
                case ModelVariableType.n:
                    break;
                case ModelVariableType.Q:
                    break;
                case ModelVariableType.Q1:
                    break;
                case ModelVariableType.Q2:
                    break;
                case ModelVariableType.Q3:
                    break;
                case ModelVariableType.f:
                    break;
                case ModelVariableType.cr:
                    break;
                case ModelVariableType.t:
                    break;
                case ModelVariableType.k:
                    break;
                case ModelVariableType.s:
                    break;
                case ModelVariableType.j:
                    break;
                case ModelVariableType.i:
                    break;
                case ModelVariableType.g:
                    break;
            }

            return list;
        }
    }
}
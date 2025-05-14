namespace Xeovis.Domain
{
    public static class DefaultModelDefinitions
    {
        public static List<CationType> GetCationsFromModelVariable(PhaseType phase, ModelVariableType variable)
        {
            return phase switch
            {
                PhaseType.g => GetVariableCations_Garnet(variable),
                PhaseType.ol => GetVariableCations_Olivine(variable),
                PhaseType.ilm => GetVariableCations_Ilmenite(variable),
                PhaseType.cpx => GetVariableCations_Cpx(variable),
                PhaseType.opx => GetVariableCations_Opx(variable),
                PhaseType.spn => GetVariableCations_Spinel(variable),
                PhaseType.ep => GetVariableCations_Epidote(variable),
                PhaseType.pl => GetVariableCations_Plagioclase(variable),
                PhaseType.bi => GetVariableCations_Biotite(variable),
                PhaseType.phe => GetVariableCations_Phengite(variable),
                _ => null
            };
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
namespace DatabaseReader.Enums
{
    public enum OxideType
    {
        SiO2,
        TiO2,
        Al2O3,
        Fe2O3,
        Cr2O3,
        FeO,
        MnO,
        MgO,
        NiO,
        CoO,
        CaO,
        Na2O,
        K2O,
        P2O5,
        SrO,
        BaO,
        H2O,
        CO2,
        SO3,
        Li2O,
        Rb2O,
        ZrO2,
        V2O3,
        Re2O3,
        //O
    }

    public static class OxideTypeExtensions
    {
        public static string GetElementFromOxideType(OxideType oxide)
        {
            switch (oxide)
            {
                case OxideType.SiO2:
                    return "Si";
                case OxideType.TiO2:
                    return "Ti";
                case OxideType.Al2O3:
                    return "Al";
                case OxideType.Fe2O3:
                    return "Fe";
                case OxideType.Cr2O3:
                    return "Cr";
                case OxideType.FeO:
                    return "Fe";
                case OxideType.MnO:
                    return "Mn";
                case OxideType.MgO:
                    return "Mg";
                case OxideType.NiO:
                    return "Ni";
                case OxideType.CoO:
                    return "Co";
                case OxideType.CaO:
                    return "Ca";
                case OxideType.Na2O:
                    return "Na";
                case OxideType.K2O:
                    return "K";
                case OxideType.P2O5:
                    return "P";
                case OxideType.SrO:
                    return "Sr";
                case OxideType.BaO:
                    return "Ba";
                case OxideType.H2O:
                    return "H";
                case OxideType.CO2:
                    return "C";
                case OxideType.SO3:
                    return "S";
                case OxideType.Li2O:
                    return "Li";
                case OxideType.Rb2O:
                    return "Rb";
                case OxideType.ZrO2:
                    return "Zr";
                case OxideType.V2O3:
                    break;
                case OxideType.Re2O3:
                    break;
                //case OxideType.O:
                //    return "O";
                default:
                    break;
            }

            return null;
        }
    }
}

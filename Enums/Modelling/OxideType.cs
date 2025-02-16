using DatabaseReader.Enums.Modelling;

namespace DatabaseReader.Enums
{
    public enum OxideType
    {
        None,
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
        O
    }

    public static class OxideTypeExtensions
    {
        public static CationType GetCationType(this OxideType oxide)
        {
            switch (oxide)
            {
                case OxideType.SiO2:
                    return CationType.Si;
                case OxideType.TiO2:
                    return CationType.Ti;
                case OxideType.Al2O3:
                    return CationType.Al;
                case OxideType.Fe2O3:
                    return CationType.Fe3;
                case OxideType.Cr2O3:
                    return CationType.Cr;
                case OxideType.FeO:
                    return CationType.Fe2;
                case OxideType.MnO:
                    return CationType.Mn;
                case OxideType.MgO:
                    return CationType.Mg;
                case OxideType.NiO:
                    return CationType.Ni;
                case OxideType.CaO:
                    return CationType.Ca;
                case OxideType.Na2O:
                    return CationType.Na;
                case OxideType.K2O:
                    return CationType.K;
                case OxideType.P2O5:
                    return CationType.P;
                default:
                    return CationType.None;
            }
        }

        public static bool IsCommon(this OxideType type)
        {
            return type == OxideType.SiO2 || type == OxideType.Al2O3 || type == OxideType.TiO2 || type == OxideType.FeO
                || type == OxideType.Fe2O3 || type == OxideType.Cr2O3 || type == OxideType.CaO || type == OxideType.MgO || type == OxideType.MnO
                || type == OxideType.Na2O || type == OxideType.K2O || type == OxideType.NiO || type == OxideType.P2O5;
        }
    }
}

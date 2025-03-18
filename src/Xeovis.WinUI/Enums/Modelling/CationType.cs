namespace DatabaseReader.Enums.Modelling
{
    public enum CationType
    {
        None,
        Si,
        Ti,
        Al,
        Fe2,
        Fe3,
        Cr,
        Mg,
        Ca,
        Mn,
        Ni,
        Na,
        K,
        P
    }

    public static class CationTypeExtensions
    {
        public static OxideType GetOxideType(this CationType cation)
        {
            switch (cation)
            {
                case CationType.Si:
                    return OxideType.SiO2;
                case CationType.Ti:
                    return OxideType.TiO2;
                case CationType.Al:
                    return OxideType.Al2O3;
                case CationType.Fe2:
                    return OxideType.FeO;
                case CationType.Fe3:
                    return OxideType.Fe2O3;
                case CationType.Mg:
                    return OxideType.MgO;
                case CationType.Ca:
                    return OxideType.CaO;
                case CationType.Mn:
                    return OxideType.MnO;
                case CationType.Ni:
                    return OxideType.NiO;
                case CationType.Na:
                    return OxideType.Na2O;
                case CationType.K:
                    return OxideType.K2O;
                case CationType.P:
                    return OxideType.P2O5;
                case CationType.Cr:
                    return OxideType.Cr2O3;
                default:
                    return OxideType.None;
            }
        }
    }
}

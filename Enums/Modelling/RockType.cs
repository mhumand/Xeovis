namespace DatabaseReader.Enums.Modelling
{
    public enum RockType
    {
        None,
        Metapelite,
        Metabasite,
        Metagreywacke,
        Peridotite,
        Serpentinite,
        Tonalite,
        Gneiss,
        Amphibolite,
        Metavolcanoclastic,
        Eclogite,
        Sediment,
        Granulite
    }

    public static class RockTypeExtensions
    {
        public static RockType GetRockType(string type)
        {
            var name = type.ToLower();
            if (name.Contains("pelite"))
                return RockType.Metapelite;
            else if (name.Contains("basite"))
                return RockType.Metabasite;
            else if (name.Contains("greywacke"))
                return RockType.Metagreywacke;
            else if (name.Contains("peridotite"))
                return RockType.Peridotite;
            else if (name.Contains("serpentinite"))
                return RockType.Serpentinite;
            else if (name.Contains("tonalite"))
                return RockType.Tonalite;
            else if (name.Contains("gneiss"))
                return RockType.Gneiss;
            else if (name.Contains("amphibolite"))
                return RockType.Amphibolite;
            else if (name.Contains("metavolcanoclastic"))
                return RockType.Metavolcanoclastic;
            else if (name.Contains("eclogite"))
                return RockType.Eclogite;
            else if (name.Contains("sediment"))
                return RockType.Sediment;
            else if (name.Contains("granulite"))
                return RockType.Granulite;
            
            return RockType.None;
        }
    }
}

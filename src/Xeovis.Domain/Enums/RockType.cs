namespace Xeovis.Domain
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
            if (name.Contains("basite"))
                return RockType.Metabasite;
            if (name.Contains("greywacke"))
                return RockType.Metagreywacke;
            if (name.Contains("peridotite"))
                return RockType.Peridotite;
            if (name.Contains("serpentinite"))
                return RockType.Serpentinite;
            if (name.Contains("tonalite"))
                return RockType.Tonalite;
            if (name.Contains("gneiss"))
                return RockType.Gneiss;
            if (name.Contains("amphibolite"))
                return RockType.Amphibolite;
            if (name.Contains("metavolcanoclastic"))
                return RockType.Metavolcanoclastic;
            if (name.Contains("eclogite"))
                return RockType.Eclogite;
            if (name.Contains("sediment"))
                return RockType.Sediment;
            if (name.Contains("granulite"))
                return RockType.Granulite;

            return RockType.None;
        }
    }
}
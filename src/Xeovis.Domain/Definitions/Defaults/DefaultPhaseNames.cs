using Xeovis.Domain.Definitions.Experiment;

namespace Xeovis.Domain.Definitions
{
    public static class DefaultPhaseNames
    {
        public static List<PhaseNameSet> GetDefaultPhaseNames()
        {
            var list = new List<PhaseNameSet>()
            {
                // Amphiboles
                new("amphibole", "amph", ["amp"]),
                new("hornblende", "hb", ["hbl", "horn", "hornb"]),
                new("gedrite", "ged"),
                new("actinolite", "act",["actino"]),

                // Pyroxenes
                new("clinopyroxene", "cpx",["clino-pyroxene"]),
                    new("augite","aug"),
                    new("pigeonite","pig"),
                    new("omphacite", "omp", ["omph"]),
                new("orthopyroxene", "opx",["ortho-pyroxene"]),

                // Epidote Group
                new("epidote", "ep", ["epi","epid"]),
                new("allanite", "allan"),
                new("zoisite", "zo"),

                // Feldspars
                new("potassium feldspar","ksp", ["kfs", "k-feldspar", "potassium-feldspar", "potassiumfeldspar"]),
                new("plagioclase", "pl", ["plag", "fsp"]),
                new("albite", "alb"),
                new("orthoclase", "orcl"),
                new("anorthite", "an"),

                // Mica Group
                new("muscovite", "mu", ["ms", "mus", "musc"]),
                new("biotite", "bi", ["bt","biot"]),
                new("phlogopite", "phl",["phlog"]),
                new("phengite", "phe", ["phen", "pheng"]),

                new("olivine", "ol"),

                // Spinel Group
                new("spinel", "spn", ["sp"]),
                new("magnetite", "mt"),
                new("hercynite", "herc", ["hc"]),

                // Pure Phases
                new("quartz", "q", ["qz", "qrtz", "sio2"]),
                new("coesite","coe", ["coes"]),
                new("water", "h2o", ["wat"]),
                new("rutile", "ru", ["rt", "rut"]),
                
                    // Aluminosilicates
                    new("kyanite", "ky"),
                    new("sillimanite","sill", ["sil"]),
                    new("andalusite","and"),
                    
                    // Carbonates
                    new("dolomite", "dol"),
                    new("calcite", "cc"),
                    new("magnesite", "mag"),
                    new("ankerite", "ank"),

                new("sphene","sph", ["titanite"]),

                new("liquid", "L", ["liq", "melt"]),
                new("fluid", "fl"),
                new("supercritical fluid", "scfl", ["sc fl", "scfluid", "sc fluid", "sc-fluid","supercritical-fluid"]),

                new("cordierite", "cd", ["crd", "cord"]),

                // Others
                new("garnet", "gt", ["g", "grt", "gar"]),

                new("ilmenite", "ilm"),

                new("chlorite","chl",["chlor"]),

                new("zircon", "zrc", ["zr","zrc"]),

                new("chloritoid", "ctd"),

                new("graphite", "gph", ["graph"]),

                new("antigorite", "atg", ["atg"]),

                new("hematite", "hem"),

                new("pyrrhotite", "pyrr", ["po"]),

                new("staurolite", "st"),

                new("apatite", "apat", ["ap"]),

                new("monazite", "monaz", ["mon"]),

                new("quench", "quench"),

                new("carbonatite", "crbt"),

                new("sulphite", "slpht"),

                new("opaque", "opaque", ["op"]),

            };

            return list;
        }
    }
}

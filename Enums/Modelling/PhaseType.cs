using System.ComponentModel;

namespace DatabaseReader.Enums
{
    public enum PhaseType
    {
        NULL,

        [Description("amphibole")]
        amph,

        [Description("actinolite")]
        act,

        [Description]
        an,

        [Description("biotite")]
        bi,

        [Description("coesite")]
        coe,

        [Description("chlorite")]
        chl,

        [Description("clinopyroxene")]
        cpx,

        [Description("pigeonite")]
        pig,

        [Description("omphacite")]
        omp,

        [Description("epidote")]
        ep,

        [Description("garnet")]
        g,

        [Description("hornblende")]
        hb,

        [Description("water")]
        H2O,

        [Description("ilmenite")]
        ilm,

        [Description("potassium feldspar")]
        ksp,

        [Description("magnetite")]
        mt,

        [Description("muscovite")]
        mu,

        [Description("olivine")]
        ol,

        [Description("orthopyroxene")]
        opx,

        [Description("plagioclase")]
        pl,

        [Description("spinel")]
        spn,

        [Description("quartz")]
        q,

        [Description("rutile")]
        ru,

        [Description("sphene")]
        sph,

        [Description("kyanite")]
        ky,

        [Description("sillimanite")]
        sill,

        [Description("andalusite")]
        and,

        [Description("liquid")]
        L,

        [Description("fluid")]
        fl,

        [Description("supercritical fluid")]
        scfl,

        [Description("phengite")]
        phe,

        [Description("zoisite")]
        zo,

        [Description("cordierite")]
        cd,

        [Description("magnesite")]
        mag,

        [Description("dolomite")]
        dol,

        [Description("calcite")]
        cc,

        [Description("zircon")]
        zrc,

        [Description("chloritoid")]
        ctd,

        [Description("graphite")]
        gph,

        [Description("antigorite")]
        atg,

        [Description("hematite")]
        hem,

        [Description("hercynite")]
        herc,

        [Description("pyrrhotite")]
        pyrr,

        [Description("gedrite")]
        ged,

        [Description("staurolite")]
        st,

        // not in ds636 - dummy phases
        [Description("allanite")]
        allan,

        [Description("apatite")]
        apat,

        [Description("monazite")]
        monaz,

        [Description("quench")]
        quench,

        [Description("carbonatite")]
        crbt,

        [Description("sulphite")]
        slpht,

        [Description("opaque")]
        opaque,

    }

    public static class PhaseTypeExtensions
    {
        public static PhaseType GetPhaseTypeByName(string name)
        {
            switch (name.ToLower())
            {
                case "amp":
                case "amph":
                case "amphibole":
                    return PhaseType.amph;
                case "act":
                case "actinolite":
                    return PhaseType.act;
                case "an":
                    return PhaseType.an;
                case "cpx":
                case "clinopyroxene":
                case "clino-pyroxene":
                    return PhaseType.cpx;
                case "opx":
                case "orthopyroxene":
                case "ortho-pyroxene":
                case "en":
                case "enstatite":
                    return PhaseType.opx;
                case "l":
                case "liq":
                case "liquid":
                case "melt":
                    return PhaseType.L;
                case "q":
                case "sio2":
                case "qz":
                case "quartz":
                case "qrtz":
                    return PhaseType.q;
                case "bi":
                case "bt":
                case "biotite":
                case "phl":
                case "phlogopite":
                    return PhaseType.bi;
                case "pl":
                case "plag":
                case "plagioclase":
                    return PhaseType.pl;
                case "ru":
                case "rut":
                case "rt":
                case "rutile":
                    return PhaseType.ru;
                case "ksp":
                case "kfs":
                case "k-feldspar":
                case "potassium-feldspar":
                case "potassium feldspar":
                case "potassiumfeldspar":
                    return PhaseType.ksp;
                case "coe":
                case "coes":
                case "coesite":
                    return PhaseType.coe;
                case "chl":
                case "chlorite":
                    return PhaseType.chl;
                case "pig":
                case "pigeonite":
                    return PhaseType.pig;
                case "omp":
                case "omph":
                case "omphacite":
                    return PhaseType.omp;
                case "ep":
                case "epidote":
                    return PhaseType.ep;
                case "g":
                case "gt":
                case "grt":
                case "garnet":
                    return PhaseType.g;
                case "hb":
                case "hbl":
                case "hornb":
                case "hornblende":
                case "al-hbl":
                case "ca-hbl":
                case "parg":
                case "pargasite":
                    return PhaseType.hb;
                case "h2o":
                case "water":
                    return PhaseType.H2O;
                case "mt":
                case "magnetite":
                case "timag":
                case "ti-mag":
                    return PhaseType.mt;
                case "ilm":
                case "ilmenite":
                    return PhaseType.ilm;
                case "mu":
                case "ms":
                case "mus":
                case "musc":
                case "muscovite":
                    return PhaseType.mu;
                case "ol":
                case "olivine":
                    return PhaseType.ol;
                case "spn":
                case "spinel":
                    return PhaseType.spn;
                case "sp":
                case "sph":
                case "sphene":
                case "titanie":
                    return PhaseType.sph;
                case "ky":
                case "kyanite":
                    return PhaseType.ky;
                case "sill":
                case "sil":
                case "sillimanite":
                    return PhaseType.sill;
                case "and":
                case "andalusite":
                    return PhaseType.and;
                case "phe":
                case "phen":
                case "phengite":
                    return PhaseType.phe;
                case "zo":
                case "zoisite":
                    return PhaseType.zo;
                case "cd":
                case "crd":
                case "cord":
                case "cordierite":
                    return PhaseType.cd;
                case "mag":
                case "magnesite":
                    return PhaseType.mag;
                case "dol":
                case "dolomite":
                    return PhaseType.dol;
                case "cc":
                case "calcite":
                    return PhaseType.cc;
                case "fl":
                case "fluid":
                    return PhaseType.fl;
                case "scfl":
                case "scfluid":
                case "sc fluid":
                case "sc-fluid":
                case "supercritical fluid":
                case "supercritical-fluid":
                case "supercriticalfluid":
                    return PhaseType.scfl;
                case "zr":
                case "zrc":
                case "zir":
                case "zircon":
                    return PhaseType.zrc;
                case "ctd":
                case "chloritoid":
                    return PhaseType.ctd;
                case "gph":
                case "graph":
                case "graphite":
                    return PhaseType.gph;
                case "atg":
                case "ant":
                case "antigorite":
                    return PhaseType.atg;
                case "hem":
                case "hematite":
                    return PhaseType.hem;
                case "hc":
                case "hercynite":
                    return PhaseType.herc;
                case "po":
                case "pyrrhotite":
                    return PhaseType.pyrr;
                case "ged":
                case "gedrite":
                    return PhaseType.ged;
                case "st":
                case "staurolite":
                    return PhaseType.st;

                // not found in ds636 - dummy phases
                case "allan":
                case "allanite":
                    return PhaseType.allan;
                case "ap":
                case "apat":
                case "apatite":
                    return PhaseType.apat;
                case "mon":
                case "monaz":
                case "monazite":
                    return PhaseType.monaz;
                case "quench":
                    return PhaseType.quench;
                case "carbonatite":
                    return PhaseType.crbt;
                case "sulph":
                case "sulphite":
                case "fe-sulph":
                    return PhaseType.slpht;
                case "op":
                case "opaque":
                    return PhaseType.opaque;
            };

            return PhaseType.NULL;
        }
    }
}

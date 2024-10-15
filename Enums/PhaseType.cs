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
        cpx2,

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

        [Description]
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

        [Description]
        sph,

        [Description("kyanite")]
        ky,

        [Description("sillimanite")]
        sill,

        [Description("andalusite")]
        and,

        [Description]
        pren,

        [Description("liquid")]
        L,

        [Description("phengite")]
        phe,

        [Description]
        zo,

        [Description("cordierite")]
        cd,
    }

    public static class PhaseTypeExtensions
    {
        public static PhaseType GetPhaseTypeByName(string name)
        {
            switch (name)
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
                    return PhaseType.opx;
                case "l":
                case "liq":
                case "liquid":
                case "fluid":
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
                    return PhaseType.hb;
                case "h2o":
                case "water":
                    return PhaseType.H2O;
                case "mt":
                    return PhaseType.mt;
                case "ilm":
                case "ilmenite":
                    return PhaseType.ilm;
                case "mu":
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
                case "sph":
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
                case "pren":
                    return PhaseType.pren;
                case "phe":
                case "phen":
                case "phengite":
                    return PhaseType.phe;
                case "zo":
                    return PhaseType.zo;
                case "cd":
                case "crd":
                case "cord":
                case "cordierite":
                    return PhaseType.cd;
                default:
                    break;
            };

            return PhaseType.NULL;
        }
    }
}

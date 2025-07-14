using Xeovis.Domain.Definitions.MCTC;

namespace Xeovis.Domain.Definitions.Defaults
{
    public static class DefaultXeosVariableNames
    {
        private static Dictionary<Guid, List<xeosVariableName>> _defaultXeosVariableNames { get; set; }

        public static void Initialize()
        {
            _defaultXeosVariableNames = new Dictionary<Guid, List<xeosVariableName>>()
            {
                { PhaseNameList.GetNameSetIdByString("garnet"), 
                    [ new("x"), new("c"), new("f"), new("cr"), new("t") ] },

                { PhaseNameList.GetNameSetIdByString("olivine"),
                    [ new("x"), new("c") ] },

                { PhaseNameList.GetNameSetIdByString("ilmenite"),
                    [ new("i"), new("m") ] },

                { PhaseNameList.GetNameSetIdByString("clinopyroxene"),
                    [ new("x"), new("o"), new("y"), new("n"), new("f"), new("cr"), new("t"), new("k") ] },

                { PhaseNameList.GetNameSetIdByString("orthopyroxene"),
                    [ new("x"), new("y"), new("c"), new("f"), new("cr"), new("t"), new("j") ] },

                { PhaseNameList.GetNameSetIdByString("spinel"),
                    [ new("x"), new("y"), new("c"), new("t") ] },

                { PhaseNameList.GetNameSetIdByString("epidote"),
                    [ new("f") ] },

                { PhaseNameList.GetNameSetIdByString("biotite"),
                    [ new("x"), new("y"), new("f"), new("t") ] },
 
                { PhaseNameList.GetNameSetIdByString("plagioclase"),
                    [ new("ca"), new("k") ] },
            };

            _defaultXeosVariableNames.ToList().ForEach(x => x.Value.ForEach(v => v.PhaseId = x.Key));
        }

        public static Dictionary<Guid, List<xeosVariableName>> GetDefaultXeosVariableNames()
        {
            return _defaultXeosVariableNames;
        }
    }
}

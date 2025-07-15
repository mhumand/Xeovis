using Xeovis.Persistence.Persistables;

namespace Xeovis.Persistence
{
    public class ProjectSettings01
    {
        public List<PhaseNameSet01> AllowedPhaseNames { get; set; }
        public Dictionary<Guid, List<xeosVariableName01>> PhaseXeosVariables { get; set; }

        public ProjectSettings01()
        {
            
        }
        public ProjectSettings01(ProjectSettings realObject)
        {
            AllowedPhaseNames = realObject.AllowedPhaseNames.Select(x => new PhaseNameSet01(x)).ToList();
            PhaseXeosVariables = realObject.PhaseXeosVariables.ToDictionary(o => o.Key, o => o.Value.Select(x => new xeosVariableName01(x)).ToList());
        }

        public ProjectSettings ConvertToRealObject()
        {
            return new ProjectSettings()
            {
                AllowedPhaseNames = AllowedPhaseNames.Select(x => x.ConvertToRealObject()).ToList(),
                PhaseXeosVariables = PhaseXeosVariables.ToDictionary(o => o.Key, o => o.Value.Select(x => x.ConvertToRealObject()).ToList())
            };
        }
    }
}

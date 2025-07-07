namespace Xeovis.Persistence
{
    public class ProjectSettings01
    {
        public List<PhaseNameSet01> AllowedPhaseNames { get; set; }

        public ProjectSettings01()
        {
            
        }
        public ProjectSettings01(ProjectSettings realObject)
        {
            AllowedPhaseNames = realObject.AllowedPhaseNames.Select(x => new PhaseNameSet01(x)).ToList();
        }

        public ProjectSettings ConvertToRealObject()
        {
            return new ProjectSettings()
            {
                AllowedPhaseNames = AllowedPhaseNames.Select(x => x.ConvertToRealObject()).ToList()
            };
        }
    }
}

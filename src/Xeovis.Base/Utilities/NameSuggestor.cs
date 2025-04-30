namespace Xeovis.Base.Utilities
{
    public static class NameSuggestor
    {
        public static string SuggestNewName_Numeric(List<string> inUseNames, string core, string spacer)
        {
            int counter = 1;
            var name = core + spacer + counter.ToString();
            while (inUseNames.Contains(name))
            {
                counter++;
                name = core + spacer + counter.ToString();
            }

            return name;
        }
    }
}

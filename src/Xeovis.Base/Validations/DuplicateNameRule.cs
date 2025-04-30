namespace Xeovis.Base.Validations
{
    public class DuplicateNameRule : IValidationRule<string>
    {
        public string ErrorMessage => "Name is already in use.";

        private readonly List<string> _existingNames;
        private readonly bool _ignoreCase;

        public DuplicateNameRule(List<string> exisitingNames, bool ignoreCaseComparison = false)
        {
            _existingNames = exisitingNames;
            _ignoreCase = ignoreCaseComparison;
        }

        public bool Validate(string value)
        {
            return !_existingNames.Any(name => string.Equals(name, value, _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }
    }
}

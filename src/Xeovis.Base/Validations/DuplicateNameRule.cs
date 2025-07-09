using System;

namespace Xeovis.Base.Validations
{
    public class DuplicateNameRule : IValidationRule<string>
    {
        private string _value;
        public string ErrorMessage => string.IsNullOrWhiteSpace(_value) ? "Name is already in use." : $"\"{_value}\" is already in use.";

        private readonly List<string> _existingNames;
        private readonly bool _ignoreCase;
        private readonly bool _usePartialComparison;
        private readonly string _partialComparisonDelimiter;

        public string CurrentNameToSplit;

        public DuplicateNameRule(List<string> exisitingNames, bool ignoreCaseComparison = false, bool usePartialComparison = false, string partialComparisonDelimiter = "")
        {
            CurrentNameToSplit = string.Empty;
            _value = string.Empty;
            _existingNames = exisitingNames;
            _ignoreCase = ignoreCaseComparison;
            _usePartialComparison = usePartialComparison;
            _partialComparisonDelimiter = partialComparisonDelimiter;
        }

        public bool Validate(string value)
        {
            List<string> existingNames = null;
            if (!string.IsNullOrWhiteSpace(CurrentNameToSplit) && _usePartialComparison)
            {
                var splitNames = CurrentNameToSplit.Split(_partialComparisonDelimiter, StringSplitOptions.TrimEntries).ToList();
                splitNames.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                existingNames = _existingNames.Except(splitNames).ToList();
            }
            else
                existingNames = _existingNames;

            var comparisonType = _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (_usePartialComparison)
            {
                var func = (string name) => value.Split(_partialComparisonDelimiter, StringSplitOptions.TrimEntries).Any(x => x.Equals(name));
                var valid = !existingNames.Any(func);
                if (!valid)
                {
                    _value = existingNames.FirstOrDefault(func);
                }

                return valid;
            }
            else
            {
                _value = value;
                return !existingNames.Any(name => string.Equals(name, value, comparisonType));
            }
        }
    }
}

namespace Xeovis.Base.Validations
{
    public class NumericRule : IValidationRule<string>
    {
        private string _errorMessage { get; set; } = string.Empty;
        public string ErrorMessage => _errorMessage;

        private double? _min;
        private double? _max;

        public NumericRule(double? min = null, double? max = null)
        {
            _min = min;
            _max = max;
        }

        public bool Validate(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                _errorMessage = "Invalid number format.";
                return false;
            }

            if ((_min.HasValue && value < _min.Value) || (_max.HasValue && value > _max.Value))
            {
                _errorMessage = $"Value must be between {_min ?? double.MinValue} and {_max ?? double.MaxValue}.";
                return false;
            }

            _errorMessage = string.Empty;
            return true;
        }

        public bool Validate(string value)
        {
            var number = DoubleExtensions.GetDoubleValueFromText(value);
            return Validate(number);
        }

        public void SetMinMaxLimits(double? min = null, double? max = null)
        {
            _min = min;
            _max = max;
        }
    }
}

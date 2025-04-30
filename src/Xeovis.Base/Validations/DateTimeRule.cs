namespace Xeovis.Base.Validations
{
    internal class DateTimeRule : IValidationRule<string>
    {
        private readonly DateTime? _min;
        private readonly DateTime? _max;

        public string _errorMessage { get; set; }
        public string ErrorMessage => _errorMessage;

        public DateTimeRule(DateTime? minDateTime = null, DateTime? maxDateTime = null)
        {
            _min = minDateTime ?? new DateTime(1900, 1, 1);
            _max = maxDateTime;
        }

        public bool Validate(DateTime value)
        {
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
            {
                _errorMessage = "Invalid date-time format.";
                return false;
            }

            if ((_min.HasValue && value < _min.Value) || (_max.HasValue && value > _max.Value))
            {
                _errorMessage = $"Value must be between {_min} and {_max}.";
                return false;
            }

            return true;
        }

        public bool Validate(string value)
        {
            var dateTime = DateTime.Parse(value);
            return Validate(dateTime);
        }
    }
}

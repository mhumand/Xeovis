namespace Xeovis.Base.Validations
{
    public class NotEmptyOrWhitespaceRule : IValidationRule<string>
    {
        public string ErrorMessage => "Value cannot be empty or whitespace.";

        public bool Validate(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}

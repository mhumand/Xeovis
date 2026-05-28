namespace Xeovis.Base.Validations;

public class MaxNameLengthRule : IValidationRule<string>
{
    private int _maxLength;
    public string ErrorMessage => $"Max name length is {_maxLength} characters.";

    public MaxNameLengthRule(int maxLength = 100)
    {
        _maxLength = maxLength;
    }
    
    public bool Validate(string value)
    {
        return value.Length <= _maxLength;
    }
}
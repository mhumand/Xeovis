namespace Xeovis.Base.Validations;

public class ValidUrlRule : IValidationRule<string>
{
    private string _errorMessage;
    public string ErrorMessage => _errorMessage;
    public bool Validate(string value)
    {
        var url = value.Trim();
        _errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(url))
        {
            _errorMessage = "URL cannot be empty.";
            return false;
        }

        string structureTestUrl = url.Replace("*", "localhost").Replace("+", "localhost");
        
        if (!Uri.TryCreate(structureTestUrl, UriKind.Absolute, out Uri? uriResult))
        {
            _errorMessage = "Invalid URL format.\nExample: http://localhost:5007";
            return false;
        }

        if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
        {
            _errorMessage = "URL must start with 'http://' or 'https://'.";
            return false;
        }

        if (uriResult.Port < 1 || uriResult.Port > 65535)
        {
            _errorMessage = "URL must contain a valid port number between 1 and 65535.";
            return false;
        }

        return true;
    }
}
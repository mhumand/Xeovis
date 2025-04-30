namespace Xeovis.Base.Validations
{
    public interface IValidationRule<T>
    {
        string ErrorMessage { get; }
        bool Validate(T value);
    }
}

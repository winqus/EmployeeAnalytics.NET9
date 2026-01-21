namespace EmpAnalytics.Application.Exceptions;

public abstract class ValidationException(IReadOnlyCollection<ValidationError> errors) : Exception("Validation failed")
{
    public IReadOnlyCollection<ValidationError> Errors { get; } = errors;
}

public abstract record ValidationError(string PropertyName, string ErrorMessage);
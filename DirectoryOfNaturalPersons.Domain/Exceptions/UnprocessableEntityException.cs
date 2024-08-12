using FluentValidation.Results;

namespace DirectoryOfNaturalPersons.Domain.Exceptions;

public class UnprocessableEntityException : Exception
{
    public ValidationResult ValidationResult { get; set; }

    public UnprocessableEntityException(string message, ValidationResult validationResult) : base(message)
    {
        ValidationResult = validationResult;
    }
}
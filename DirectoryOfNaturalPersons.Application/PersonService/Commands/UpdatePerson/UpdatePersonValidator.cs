using DirectoryOfNaturalPersons.Application.Interface;
using DirectoryOfNaturalPersons.Domain.Constants;
using DirectoryOfNaturalPersons.Domain.Enums;
using FluentValidation;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.UpdatePerson;

public class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
{
    private readonly IResourceManagerService _resourceManagerService;

    public UpdatePersonValidator(IResourceManagerService resourceManagerService)
    {
        _resourceManagerService = resourceManagerService;
        RuleFor(x => x.FirstName)
            .NotNull()
            .WithMessage(GetResourceString(ValidationMessages.FirstNameIsRequired))
            .NotEmpty()
            .Length(2, 50)
            .WithMessage(GetResourceString(ValidationMessages.FirstNameInvalidLength));

        RuleFor(x => x.LastName)
            .NotNull()
            .WithMessage(GetResourceString(ValidationMessages.LastNameIsRequired))
            .NotEmpty()
            .Length(2, 50)
            .WithMessage(GetResourceString(ValidationMessages.LastNameInvalidLength));

        RuleFor(x => x.Gender)
            .Must(x => Enum.TryParse<Gender>(x.ToString(), out _))
            .WithMessage(GetResourceString(ValidationMessages.GenderInvalidValue));

        RuleFor(x => x.PersonalId)
            .Matches(@"^\d{11}$")
            .WithMessage(GetResourceString(ValidationMessages.PersonalIdMustContain11NumericCharacters));

        RuleFor(x => x.BirthDate)
            .NotNull()
            .WithMessage(ValidationMessages.BirthDateIsRequired)
            .LessThan(DateTime.Now.AddYears(-18))
            .WithMessage(GetResourceString(ValidationMessages.PersonMustBeAtLeast18YearsOldToRegister));

        RuleFor(x => x.PhoneNumbers)
            .NotNull()
            .WithMessage(ValidationMessages.PhoneNumbersCannotBeNull)
            .Must(x => x.Any())
            .WithMessage(GetResourceString(ValidationMessages.AtLeastOnePhoneNumberMustBeProvided));

        RuleForEach(x => x.PhoneNumbers)
            .ChildRules(phoneNumber =>
            {
                phoneNumber.RuleFor(x => x.Number)
                    .Length(4, 50)
                    .WithMessage(GetResourceString(ValidationMessages.NumberInvalidLength));

                phoneNumber.RuleFor(x => x.NumberType)
                    .Must(x => Enum.TryParse<PhoneNumberType>(x.ToString(), out _))
                    .WithMessage(GetResourceString(ValidationMessages.NumberInvalidType));
            });
    }

    private string? GetResourceString(string key)
    {
        return _resourceManagerService.GetString(key);
    }
}
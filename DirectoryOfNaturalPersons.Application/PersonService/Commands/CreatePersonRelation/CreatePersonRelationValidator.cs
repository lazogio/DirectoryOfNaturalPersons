using DirectoryOfNaturalPersons.Domain.Enums;
using FluentValidation;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePersonRelation;

public class CreatePersonRelationValidator : AbstractValidator<CreatePersonRelationCommand>
{
    public CreatePersonRelationValidator()
    {
        RuleFor(x => x.PersonId)
            .NotNull()
            .WithMessage("Person Id Required");

        RuleFor(x => x.RelatedPersonId)
            .NotNull()
            .WithMessage("RelatedPersonId Required");

        RuleFor(x => x.RelationType)
            .Must(x => Enum.TryParse<RelationType>(x.ToString(), out _))
            .NotEmpty()
            .WithMessage("RelationType must be provided.");
    }
}
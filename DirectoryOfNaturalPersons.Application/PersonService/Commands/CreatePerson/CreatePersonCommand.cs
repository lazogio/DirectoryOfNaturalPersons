using DirectoryOfNaturalPersons.Domain.Enums;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePerson;

public sealed record CreatePersonCommand(
    string FirstName,
    string LastName,
    string PersonalId,
    DateTime BirthDate,
    int CityId,
    Gender Gender,
    IEnumerable<PhoneNumberModel> PhoneNumbers) : IRequest<PersonModel>;
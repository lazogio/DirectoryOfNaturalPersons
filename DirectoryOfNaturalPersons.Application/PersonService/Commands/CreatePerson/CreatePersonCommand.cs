using System.Text.Json.Serialization;
using DirectoryOfNaturalPersons.Application.Models;
using DirectoryOfNaturalPersons.Domain.Enums;
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
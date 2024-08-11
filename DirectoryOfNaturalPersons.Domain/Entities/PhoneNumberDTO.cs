using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Domain.Entities;

public class PhoneNumberDTO : EntityDTO
{
    public string Number { get; set; }
    public PhoneNumberType NumberType { get; set; }
    public int PersonId { get; set; }
    public PersonDTO PersonDto { get; set; }
}
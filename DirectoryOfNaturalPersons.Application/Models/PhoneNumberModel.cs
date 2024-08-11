using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Application.Models;

public class PhoneNumberModel
{
    public string Number{ get; set; }
    public PhoneNumberType NumberType{ get; set; }
}
using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Domain.Models;

public class PhoneNumberModel
{
    public string Number{ get; set; }
    public PhoneNumberType NumberType{ get; set; }
}
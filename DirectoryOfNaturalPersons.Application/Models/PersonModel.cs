using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Application.Models;

public class PersonModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalId { get; set; }
    public DateTime BirthDate { get; set; }
    public String City { get; set; }
    public Gender gender { get; set; }
    public IEnumerable<PhoneNumberModel> PhoneNumbers { get; set; }
}
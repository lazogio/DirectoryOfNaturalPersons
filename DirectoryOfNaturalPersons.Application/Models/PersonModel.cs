using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Application.Models;

public class PersonModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonId { get; set; }
    public DateTime BirthDate { get; set; }
    public int CityId { get; set; }
    public Gender gender { get; set; }
    public IEnumerable<PhoneNumberModel> PhoneNumber { get; set; }
}
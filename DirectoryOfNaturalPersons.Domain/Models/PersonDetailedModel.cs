using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Domain.Models;

public class PersonDetailedModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalId { get; set; }
    public DateTime BirthDate { get; set; }
    public string City { get; set; }
    public Gender Gender { get; set; }
    public ICollection<RelatedPersonsModel> RelatedPersons { get; set; }
    public ICollection<PhoneNumberModel> PhoneNumbers { get; set; }
    public string? FilePath { get; set; }

}
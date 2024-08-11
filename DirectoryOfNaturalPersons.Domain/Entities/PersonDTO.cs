using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Domain.Entities;

public class PersonDTO : EntityDTO
{
    public PersonDTO()
    {
        RelatedPersons = new List<PersonRelationDTO>();
        RelatedToPersons = new List<PersonRelationDTO>();
        PhoneNumbers = new List<PhoneNumberDTO>();
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalId { get; set; }
    public DateTime BirthDate { get; set; }
    public int CityId { get; set; }
    public Gender Gender { get; set; }
    public ICollection<PersonRelationDTO> RelatedPersons { get; set; }
    public ICollection<PersonRelationDTO> RelatedToPersons { get; set; }
    public ICollection<PhoneNumberDTO> PhoneNumbers { get; set; }
}
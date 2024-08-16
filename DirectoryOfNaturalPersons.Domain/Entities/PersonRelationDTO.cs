using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Domain.Entities;

public class PersonRelationDTO
{
    public int PersonId { get; set; }
    public int RelatedPersonId { get; set; }
    public RelationType RelationType { get; set; }
    public PersonDTO PersonDto { get; set; }
    public PersonDTO RelatedPersonDto { get; set; }
}
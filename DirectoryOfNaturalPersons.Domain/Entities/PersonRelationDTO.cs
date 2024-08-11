using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Domain.Entities;

public class PersonRelationDTO
{
    public PersonRelationDTO()
    {
    }

    public PersonRelationDTO(PersonDTO personDto, PersonDTO relatedPersonDto, RelationType relationType)
    {
        PersonDto = personDto;
        RelatedPersonDto = relatedPersonDto;
        RelationType = relationType;
    }

    public int PersonId { get; set; }
    public int RelatedPersonId { get; set; }
    public RelationType RelationType { get; set; }
    public PersonDTO PersonDto { get; set; }
    public PersonDTO RelatedPersonDto { get; set; }
}
using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Domain.Models;

public class RelatedPersonsModel
{
    public int RelatedPersonId { get; set; }
    public RelationType RelationType { get; set; }
}
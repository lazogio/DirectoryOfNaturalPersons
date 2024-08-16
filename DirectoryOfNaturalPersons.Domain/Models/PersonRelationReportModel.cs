using DirectoryOfNaturalPersons.Domain.Enums;

namespace DirectoryOfNaturalPersons.Domain.Models;

public class PersonRelationReportModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public RelationType RelationType { get; set; }
    public int Count { get; set; }
}
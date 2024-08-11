namespace DirectoryOfNaturalPersons.Domain.Entities;

public abstract class EntityDTO
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}
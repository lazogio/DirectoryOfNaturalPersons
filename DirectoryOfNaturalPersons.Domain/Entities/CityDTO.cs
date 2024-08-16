namespace DirectoryOfNaturalPersons.Domain.Entities;

public class CityDTO : EntityDTO
{
    public string NameKa { get; set; }
    public string NameEn { get; set; }
    public string Location { get; set; }

    public void SetCreateDate()
    {
        CreateDate = DateTime.Now;
    }
}
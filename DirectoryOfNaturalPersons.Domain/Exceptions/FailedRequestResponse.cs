using System.Text.Json;

namespace DirectoryOfNaturalPersons.Domain.Exceptions;

public class FailedRequestResponse
{
    public string? Message { get; set; }

    public string? ErrorCode { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
using System.Net;

namespace DirectoryOfNaturalPersons.Domain.Models;

public class ResponseModel
{
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public ResponseModel(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
}
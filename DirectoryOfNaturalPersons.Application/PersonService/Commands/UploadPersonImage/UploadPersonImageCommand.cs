using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.UploadPersonImage;

public class UploadPersonImageCommand : IRequest<ResponseModel>
{
    public int Id { get; set; }
    public IFormFile? File { get; set; }
}
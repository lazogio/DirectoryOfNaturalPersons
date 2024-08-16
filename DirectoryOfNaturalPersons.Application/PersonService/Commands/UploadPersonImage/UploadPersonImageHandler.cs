using System.Net;
using DirectoryOfNaturalPersons.Application.Interface;
using DirectoryOfNaturalPersons.Domain.Constants;
using DirectoryOfNaturalPersons.Domain.Exceptions;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.UploadPersonImage;

public class UploadPersonImageHandler : IRequestHandler<UploadPersonImageCommand, ResponseModel>
{
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;

    public UploadPersonImageHandler(IPersonRepository repository, IResourceManagerService resourceManagerService)
    {
        _repository = repository;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<ResponseModel> Handle(UploadPersonImageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.File == null || request.File.Length == 0)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.NoFileIsSelected);
                throw new Exception(message);
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(request.File.FileName);

            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                throw new InvalidOperationException(
                    _resourceManagerService.GetString(ValidationMessages.InvalidFileType));
            }

            var person = await _repository.GerPersonByIdAsync(request.Id, cancellationToken);
            if (person is null)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
                throw new NotFoundException(message + $"{request.Id}", true);
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var fileName = $"{person.FirstName}_{person.LastName}_{person.Id}.jpg";
            var path = Path.Combine(filePath, fileName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            await using FileStream stream = new(path, FileMode.Create);
            await request.File.CopyToAsync(stream, cancellationToken);

            return new ResponseModel("Image uploaded successfully", HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return new ResponseModel(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}
using System.ComponentModel.DataAnnotations;
using DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePerson;
using DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePersonRelation;
using DirectoryOfNaturalPersons.Application.PersonService.Commands.DeletePerson;
using DirectoryOfNaturalPersons.Application.PersonService.Commands.DeletePersonRelation;
using DirectoryOfNaturalPersons.Application.PersonService.Commands.UpdatePerson;
using DirectoryOfNaturalPersons.Application.PersonService.Commands.UploadPersonImage;
using DirectoryOfNaturalPersons.Application.PersonService.Queries.GetPerson;
using DirectoryOfNaturalPersons.Application.PersonService.Queries.GetPersonById;
using DirectoryOfNaturalPersons.Domain.GenericModel;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryOfNaturalPersons.Controllers;

[Route("v1/[controller]/[action]")]
[ApiController]
public class PersonController : Controller
{
    private readonly ISender _sender;

    public PersonController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }

    [Route("{id}")]
    [HttpGet]
    [ProducesResponseType(typeof(List<PersonDetailedModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPersonById([Required] int id, CancellationToken cancellationToken)
    {
        var query = new GetPersonByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePersonById([Required] int id, CancellationToken cancellationToken)
    {
        var query = new DeletePersonCommand(id);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateRelation([FromBody] CreatePersonRelationCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }

    [Route("{personId}/{relatedPersonId}")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePersonRelation([Required] int personId, int relatedPersonId,
        CancellationToken cancellationToken)
    {
        var query = new DeletePersonRelationCommand(personId, relatedPersonId);
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePerson([FromBody] UpdatePersonCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadPersonImage([Required] int id, [Required] IFormFile file,
        CancellationToken cancellationToken)
    {
        var query = new UploadPersonImageCommand { Id = id, File = file };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PersonDetailedModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersons([FromQuery] GetPersonsSearchQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }
}
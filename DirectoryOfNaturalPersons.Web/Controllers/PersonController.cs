using DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePerson;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryOfNaturalPersons.Controllers;

[Route("api/[controller]/[action]")]
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
}
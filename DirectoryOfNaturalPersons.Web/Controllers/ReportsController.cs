using DirectoryOfNaturalPersons.Application.PersonService.Queries.GetPersonsRelations;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryOfNaturalPersons.Controllers;

[Route("v1/[controller]/[action]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ISender _sender;

    public ReportsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IQueryable<PersonRelationReportModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RelatedPersons(CancellationToken cancellationToken)
    {
        var query = new GetPersonsRelationsQuery();
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}
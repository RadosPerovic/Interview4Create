using Interview4Create.Project.Application.UseCases.Companies.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Interview4Create.Project.API.Controllers;

[Route("api/[controller]")]
public class CompaniesController : ApiController
{
    public CompaniesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCompanyCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCompanyCommandResponse>> CreateCompany(CreateCompanyCommandRequest request)
    {
        var result = await _mediator.Send(request);

        if (!result.IsSuccessful)
        {
            return GetErrorResponse(result.Errors);
        }

        return Ok(result.Data);
    }
}

using System.Net;
using Interview4Create.Project.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Interview4Create.Project.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    protected readonly IMediator _mediator;

    public ApiController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected ActionResult GetErrorResponse(IEnumerable<Error> errors)
    {
        var response = new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            title = "One or more validation errors occurred.",
            status = HttpStatusCode.BadRequest,
            errors
        };

        return BadRequest(response);
    }
}

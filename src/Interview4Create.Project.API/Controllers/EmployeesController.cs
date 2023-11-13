using Interview4Create.Project.Application.UseCases.Employees.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Interview4Create.Project.API.Controllers;

[Route("api/[controller]")]
public class EmployeesController : ApiController
{
    public EmployeesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateEmployeeCommandResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateEmployeeCommandResponse>> CreateEmployee(CreateEmployeeCommandRequest request)
    {
        var result = await _mediator.Send(request);

        if (!result.IsSuccessful)
        {
            return GetErrorResponse(result.Errors);
        }

        //Should be return CreatedAtRoute.. (status code should be 201) but we don't have GetById endpoint
        return Ok(result.Data);
    }
}

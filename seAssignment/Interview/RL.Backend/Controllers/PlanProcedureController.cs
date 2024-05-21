using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Commands;
using RL.Data;
using RL.Data.DataModels;
using MediatR;
using RL.Backend.Models;

namespace RL.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PlanProcedureController : ControllerBase
{
    private readonly ILogger<PlanProcedureController> _logger;
    private readonly RLContext _context;
    private readonly IMediator _mediator;

    public PlanProcedureController(ILogger<PlanProcedureController> logger, RLContext context, IMediator mediator)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [EnableQuery]
    public IEnumerable<PlanProcedure> Get()
    {
        return _context.PlanProcedures;
    }

    [HttpPost("AddUsersToPlanProcedure")]
    public async Task<IActionResult> AddUsersToPlanProcedure([FromBody] AddUsersToPlanProcedureCommand command, CancellationToken token)
    {
        var response = await _mediator.Send(command, token);

        return response.ToActionResult();
    }

    [HttpGet("{planId}/{procedureId}/PlanProcedureUsers")]
    [EnableQuery]
    public async Task<IActionResult> GetAssignedUsersToPlanProcedure(int planId, int procedureId)
    {
        var planProcedure = await _context.PlanProcedures
            .Include(plp => plp.PlanProcedureUsers)
            .FirstOrDefaultAsync(plp => plp.PlanId == planId && plp.ProcedureId == procedureId);

        if (planProcedure == null)
            return NotFound();

        var UserIds = planProcedure.PlanProcedureUsers.Select(u => u.UserId).ToList();

        return Ok(UserIds);
    }
}

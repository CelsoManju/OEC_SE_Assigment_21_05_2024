using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;
using RL.Data.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RL.Backend.Commands.Handlers.Plans
{
    public class AddUsersToPlanProcedureCommandHandler : IRequestHandler<AddUsersToPlanProcedureCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;

        public AddUsersToPlanProcedureCommandHandler(RLContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<Unit>> Handle(AddUsersToPlanProcedureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                
                if (request.PlanId < 1 )
                    return ApiResponse<Unit>.Fail(new BadRequestException("Error: Invalid PlanId. Please provide a valid PlanId."));
                if (request.ProcedureId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Error: Invalid ProcedureId. Please provide a valid ProcedureId."));
                if (request.UserIds == null || request.UserIds.Count < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Error: UserIds list is empty or null."));
                


                
                var plan = await _context.Plans
                    .Include(p => p.PlanProcedures)
                    .ThenInclude(p=>p.PlanProcedureUsers)
                    .FirstOrDefaultAsync(p => p.PlanId == request.PlanId);

                var procedure = await _context.Procedures.FirstOrDefaultAsync(p => p.ProcedureId == request.ProcedureId);

                var users = await _context.Users
                    .Where(u => request.UserIds.Contains(u.UserId))
                    .ToListAsync();

                if (plan == null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"Error: PlanId {request.PlanId} not found."));
                if (procedure == null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"Error: ProcedureId {request.ProcedureId} not found."));
                var planProcedure = plan.PlanProcedures.FirstOrDefault(p => p.ProcedureId == procedure.ProcedureId);
                if (planProcedure == null)
                {
                    planProcedure = new PlanProcedure
                    {
                        ProcedureId = procedure.ProcedureId,
                        PlanId = request.PlanId,
                        PlanProcedureUsers = new List<User>()
                    };
                    plan.PlanProcedures.Add(planProcedure);
                }
                else
                {

                    planProcedure.PlanProcedureUsers.Clear();
                }

                if (users.Count != request.UserIds.Count)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Error: Some users were not found."));

                
                foreach (var user in users)
                {
                    planProcedure.PlanProcedureUsers.Add(user);
                }

                
                await _context.SaveChangesAsync(cancellationToken);

                return ApiResponse<Unit>.Succeed(Unit.Value);
            }
            catch (Exception ex)
            {
               
                return ApiResponse<Unit>.Fail(ex);
            }
        }
    }
}

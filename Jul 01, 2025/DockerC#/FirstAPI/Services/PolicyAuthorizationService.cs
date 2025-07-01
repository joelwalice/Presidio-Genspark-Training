using FirstAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Services
{
    public class MinYearsExp : IAuthorizationRequirement
    {
        public int YoE { get; }

        public MinYearsExp(int yoe)
        {
            YoE = yoe;
        }
    }

    public class PolicyAuthorizationService : AuthorizationHandler<MinYearsExp>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinYearsExp requirement)
        {
            if (context.User.HasClaim(c => c.Type == "YearsOfExperience"))
            {
                var experienceClaim = context.User.FindFirst(c => c.Type == "YoEOfExperience");
                if (experienceClaim != null && int.TryParse(experienceClaim.Value, out int yearsOfExperience))
                {
                    if (yearsOfExperience >= requirement.YoE)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
using JumpingUnicorn.Database;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JumpingUnicorn.Policy
{
    public class SetUsernameRequirmentHandler : AuthorizationHandler<SetUsernameRequirment>
    {
        private readonly FirebaseContext db;
        public SetUsernameRequirmentHandler(FirebaseContext _db) {
            db = _db;
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, SetUsernameRequirment requirement)
        {
            string id = context.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            if (await db.DoesUserHasUsername(id))
            {
                context.Succeed(requirement);
            }

            

            return Task.CompletedTask;
        }
    }
}

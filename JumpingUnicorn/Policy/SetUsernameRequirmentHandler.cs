using JumpingUnicorn.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace JumpingUnicorn.Policy
{

    /// <summary>
    /// This class checks if the user has a username
    /// </summary>
    public class SetUsernameRequirmentHandler : AuthorizationHandler<SetUsernameRequirment>
    {
        private readonly FirebaseContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SetUsernameRequirmentHandler(FirebaseContext _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            _httpContextAccessor = httpContextAccessor;
        }



        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, SetUsernameRequirment requirement)
        {
            string? id = context.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

            
            if (string.IsNullOrEmpty(id))
            {
                context.Fail(new AuthorizationFailureReason(this, "You have not set your username"));
                return Task.CompletedTask;
            }

            if (await db.DoesUserHasUsernameAsync(id))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail(new AuthorizationFailureReason(this, "You have not set your username"));
            
            return Task.CompletedTask;
        }
    }
}

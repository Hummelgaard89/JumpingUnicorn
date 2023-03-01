using Microsoft.AspNetCore.Authorization;

namespace JumpingUnicorn.Policy
{
    public class SetUsernameRequirment : IAuthorizationRequirement
    {
        public bool UsernameSet { get;}

        public SetUsernameRequirment(bool usernameSet)
        {
            UsernameSet = usernameSet;
        }
    }
}

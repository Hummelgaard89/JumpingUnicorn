using Microsoft.AspNetCore.Authorization;

namespace JumpingUnicorn.Policy
{
    /// <summary>
    /// This class is the requirment for if an user has set their username
    /// </summary>
    public class SetUsernameRequirment : IAuthorizationRequirement
    {
        public bool UsernameSet { get;}

        public SetUsernameRequirment(bool usernameSet)
        {
            UsernameSet = usernameSet;
        }
    }
}

using Microsoft.AspNetCore.Authorization;

namespace KonferansPortal
{
    public class IsEgitmenRequirement : IAuthorizationRequirement
    {

        public IsEgitmenRequirement()
        {
        }
    }
}

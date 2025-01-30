using Microsoft.AspNetCore.Authorization;

namespace KonferansPortal
{
    public class IsEgitmenOrKatilimciRequirement : IAuthorizationRequirement
    {

        public IsEgitmenOrKatilimciRequirement()
        {
        }
    }
}

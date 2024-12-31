using Microsoft.AspNetCore.Authorization;

namespace KonferansPortal
{
    public class IsKatilimciRequirement : IAuthorizationRequirement
    {

        public IsKatilimciRequirement()
        {
        }
    }
}

using Microsoft.AspNetCore.Authorization;

namespace KonferansPortal
{
    public class IsOnKayitliRequirement : IAuthorizationRequirement
    {

        public IsOnKayitliRequirement()
        {
        }
    }
}

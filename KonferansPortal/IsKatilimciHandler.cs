using Microsoft.AspNetCore.Authorization;
using KonferansPortal.Data;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using KonferansPortal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace KonferansPortal
{
    public class IsKatilimciHandler : AuthorizationHandler<IsKatilimciRequirement>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Uye> _userManager;
        private readonly ILogger<IsKatilimciHandler> _logger;

        public IsKatilimciHandler(AppDbContext context, UserManager<Uye> userManager, ILogger<IsKatilimciHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsKatilimciRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }

            if (context.Resource is AuthorizationFilterContext authContext)
            {
                var routeValues = authContext.RouteData.Values;
                if (routeValues.TryGetValue("konferansId", out var konferansIdValue) && int.TryParse(konferansIdValue.ToString(), out int konferansId))
                {
                    var user = await _userManager.GetUserAsync(context.User);
                    if (user == null)
                    {
                        return;
                    }

                    var konferans = await _context.Konferanslar
                        .Include(k => k.Katilimcilar)
                        .FirstOrDefaultAsync(k => k.Id == konferansId);

                    if (konferans != null && konferans.Katilimcilar.Any(k => k.Id == user.Id))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        _logger.LogWarning("User {UserId} is not a participant of conference {KonferansId}", user.Id, konferansId);
                    }
                }
                else
                {
                    _logger.LogWarning("Missing or invalid konferansId in route data");
                }
            }
            else
            {
                _logger.LogWarning("Authorization context resource is not an AuthorizationFilterContext");
            }
        }
    }
}
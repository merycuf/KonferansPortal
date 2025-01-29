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
    public class IsEgitmenHandler : AuthorizationHandler<IsEgitmenRequirement>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Uye> _userManager;
        private readonly ILogger<IsEgitmenHandler> _logger;

        public IsEgitmenHandler(AppDbContext context, UserManager<Uye> userManager, ILogger<IsEgitmenHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEgitmenRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }

            if (!(context.Resource is HttpContext httpContext)) throw new InvalidOperationException("DefaultHttpContext expected");
            {
                //var routeValues = authContext.RouteData.Values;
                //if (routeValues.TryGetValue("konferansId", out var konferansIdValue) && int.TryParse(konferansIdValue.ToString(), out int konferansId))
                if (Int32.TryParse((String)httpContext.Request.RouteValues["id"], out int konferansId))
                {
                    var user = await _userManager.GetUserAsync(context.User);
                    if (user == null)
                    {
                        return;
                    }

                    var konferans = await _context.Konferanslar
                        .Include(k => k.Egitmenler)
                        .FirstOrDefaultAsync(k => k.Id == konferansId);

                    if (konferans != null && konferans.Egitmenler.Any(e => e.Id == user.Id))
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
        }
    }
}
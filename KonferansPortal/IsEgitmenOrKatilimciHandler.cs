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
    public class IsEgitmenOrKatilimciHandler : AuthorizationHandler<IsEgitmenOrKatilimciRequirement>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Uye> _userManager;
        private readonly ILogger<IsEgitmenOrKatilimciHandler> _logger;


        public IsEgitmenOrKatilimciHandler(AppDbContext context, UserManager<Uye> userManager, ILogger<IsEgitmenOrKatilimciHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEgitmenOrKatilimciRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }

            if (!(context.Resource is HttpContext httpContext)) throw new InvalidOperationException("DefaultHttpContext expected");
            
            if (Int32.TryParse((String)httpContext.Request.RouteValues["id"], out int konferansId))
            {
                var user = await _userManager.GetUserAsync(context.User);
                if (user == null)
                {
                    return;
                }

                var konferans = await _context.Konferanslar
                    .Include(k => k.Katilimcilar)
                    .Include(k => k.Egitmenler)
                    .ThenInclude(e => e.UyeModel)
                    .FirstOrDefaultAsync(k => k.Id == konferansId);

                if (konferans != null && (konferans.Katilimcilar.Any(k => k.Id == user.Id) || konferans.Egitmenler.Any(k => k.UyeModel.Id==user.Id)) )
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
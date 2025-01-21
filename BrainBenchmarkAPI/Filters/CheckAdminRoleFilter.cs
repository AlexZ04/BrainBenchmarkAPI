using BrainBenchmarkAPI.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BrainBenchmarkAPI.Filters
{
    public class CheckAdminRoleFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly JwtSecurityTokenHandler _tokenHander = new JwtSecurityTokenHandler();

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var token = await context.HttpContext.GetTokenAsync("access_token");

            var _context = context.HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;

            if (_context == null)
            {
                SetError(context);
                return;
            }

            var jwtToken = _tokenHander.ReadJwtToken(token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                SetError(context);
                return;
            }

            var userIdGuid = new Guid(userId);

            var isUserAdmin = _context.AdminList.FirstOrDefault(a => a.Id == userIdGuid);

            if (isUserAdmin == null) SetError(context);
        }

        private void SetError(AuthorizationFilterContext context)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            context.Result = new UnauthorizedObjectResult(new
            {
                Message = "Only admins can do that!"
            });
        }
    }
}

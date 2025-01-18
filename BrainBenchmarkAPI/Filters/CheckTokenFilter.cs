using BrainBenchmarkAPI.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BrainBenchmarkAPI.Filters
{
    public class CheckTokenFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly DataContext _context;

        public CheckTokenFilter(DataContext context)
        {
            _context = context;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context) 
        {
            var token = await context.HttpContext.GetTokenAsync("access_token");
            //token = token.Result;

            var blackToken = _context.BlacklistTokens.FirstOrDefault(t => t.Token == token);

            if (blackToken != null) {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                context.Result = new UnauthorizedObjectResult(new
                {
                    Message = "User unauthorized!"
                });
            }
        }
    }
}

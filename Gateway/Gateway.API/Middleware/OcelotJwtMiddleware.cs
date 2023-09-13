using System.Security.Claims;
using Ocelot.Authorization;
using Ocelot.Middleware;

namespace Gateway.API.Middleware
{
    public class OcelotJwtMiddleware
    {
        public static async Task Authorize(HttpContext httpContext, Func<Task> next)
        {
            if (ValidateRole(httpContext)) //  && ValidateScope(httpContext))
            {
                await next.Invoke();
            }
            else
            {
                httpContext.Response.StatusCode = 403;
                httpContext.Items.SetError(new UnauthorizedError($"Fail to authorize"));
            }
        }

        private static bool ValidateRole(HttpContext ctx)
        {
            var downStreamRoute = ctx.Items.DownstreamRoute();
            if (downStreamRoute.AuthenticationOptions.AuthenticationProviderKey == null) return true;

            Claim[] userClaims = ctx.User.Claims.ToArray();

            Dictionary<string, string> requiredAuthorizationClaims = downStreamRoute.RouteClaimsRequirement;

            foreach (KeyValuePair<string, string> requiredAuthorizationClaim in requiredAuthorizationClaims)
            {
                if (ValidateIfStringIsRole(requiredAuthorizationClaim.Key))
                {
                    foreach (var requiredClaimValue in requiredAuthorizationClaim.Value.Split(","))
                    {
                        foreach (Claim userClaim in userClaims)
                        {
                            if (ValidateIfStringIsRole(userClaim.Type) && requiredClaimValue.Equals(userClaim.Value))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        
        private static bool ValidateIfStringIsRole(string role)
        {
            return role.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/role") || role.Equals("Role") ||
                   role.Equals("role");
        }
    }
}

using System.Linq;
using System.Security.Claims;

namespace Darts.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAnonnymous(this ClaimsPrincipal claimsPrincipal)
        {
            bool result = !claimsPrincipal.Claims.Any();
            return result;
        }

        public static long GetId(this ClaimsPrincipal claimsPrincipal)
        {
            string id = claimsPrincipal.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Sid)).Value;
            return long.Parse(id);
        }
    }
}

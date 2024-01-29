using System.Security.Claims;

namespace NotesAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int Id(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.Claims.First(c => c.Type == "Id").Value);
        }
    }
}

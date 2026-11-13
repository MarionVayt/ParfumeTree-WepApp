using System.Security.Claims;

namespace ParfumTree.WebApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue("sub");
        return int.TryParse(value, out var id)
            ? id
            : throw new UnauthorizedAccessException("У токені немає коректного id користувача.");
    }
}
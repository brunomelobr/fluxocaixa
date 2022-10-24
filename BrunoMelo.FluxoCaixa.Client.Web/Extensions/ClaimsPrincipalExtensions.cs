using System.Security.Claims;

namespace BrunoMelo.FluxoCaixa.Client.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.Email);

    public static string GetName(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.Name);

    public static string GetSurname(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.Surname);

    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
       => int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
}
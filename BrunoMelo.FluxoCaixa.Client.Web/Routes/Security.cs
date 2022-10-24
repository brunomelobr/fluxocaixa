namespace BrunoMelo.FluxoCaixa.Client.Web.Routes;

public static class Security
{
    private const string Base = "/security";

    public static string Get(int userId)
    {
        return $"{Base}/{userId}";
    }

    public static string GetUserRoles(int userId)
    {
        return $"{Base}/users/{userId}/roles";
    }

    public static string GetUserProfile(int userId)
    {
        return $"{Base}/users/{userId}/profile";
    }

    public static string GetPermissions(int roleId)
    {
        return $"{Base}/role/{roleId}/permissions";
    }

    public const string Login = "/login";
    public const string Account = "/account";
    public const string ForgotPassword = Base + "/forgot-password";
    public const string Register = "/register";

    public const string Roles = Base + "/roles";
    public const string Users = Base + "/users";
}
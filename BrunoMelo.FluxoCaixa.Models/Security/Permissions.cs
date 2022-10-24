using NuvTools.Security.Extensions;
using System.Collections.Generic;
using System.Security.Claims;

namespace BrunoMelo.FluxoCaixa.Models.Security;
public static class Permissions
{
    public static class Conta
    {
        public const string View = "Conta.View";
        public const string Create = "Conta.Create";
        public const string Edit = "Conta.Edit";
        public const string Delete = "Conta.Delete";
    }

    public static class Transacao
    {
        public const string View = "Transacao.View";
        public const string Create = "Transacao.Create";
        public const string Edit = "Transacao.Edit";
        public const string Delete = "Transacao.Delete";
    }

    public static class Users
    {
        public const string View = "Users.View";
        public const string Create = "Users.Create";
        public const string Edit = "Users.Edit";
        public const string Delete = "Users.Delete";
    }

    public static class Roles
    {
        public const string View = "Roles.View";
        public const string Create = "Roles.Create";
        public const string Edit = "Roles.Edit";
        public const string Delete = "Roles.Delete";
    }

    public static List<Claim> GetAllPermissions()
    {
        var allPermissions = new List<Claim>();

        #region GetPermissions

        allPermissions.AddPermissionByClass(typeof(Users));
        allPermissions.AddPermissionByClass(typeof(Roles));
        allPermissions.AddPermissionByClass(typeof(Conta));
        allPermissions.AddPermissionByClass(typeof(Transacao));

        #endregion GetPermissions

        return allPermissions;
    }

}
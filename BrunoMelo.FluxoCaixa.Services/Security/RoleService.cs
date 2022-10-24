using BrunoMelo.FluxoCaixa.Models.Data.Seguranca;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuvTools.Common.ResultWrapper;
using NuvTools.Resources;
using NuvTools.Security.Identity.AspNetCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Services.Security
{
    public class RoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleService(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id.ToString());
            if (existingRole.Name != "Administrator" && existingRole.Name != "Basic")
            {
                //TODO Check if Any Users already uses this Role
                bool roleIsNotUsed = true;
                var allUsers = await _userManager.Users.ToListAsync();
                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                    {
                        roleIsNotUsed = false;
                    }
                }
                if (roleIsNotUsed)
                {
                    await _roleManager.DeleteAsync(existingRole);
                    return Result.Success($"Role {existingRole.Name} deleted.");
                }
                else
                {
                    return Result.Fail($"Not allowed to delete {existingRole.Name} Role as it is being used.");
                }
            }
            else
            {
                return Result.Fail($"Not allowed to delete {existingRole.Name} Role.");
            }
        }

        public IQueryable<Role> GetAllAsync()
        {
            return _roleManager.Roles;
        }

        public async Task<Role> GetAsync(int id)
        {
            return await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Role> GetWithPermissionAsync(int id)
        {
            var role = await GetAsync(id);
            role.Claims = (await GetPermissionsAsync(id))
                        .Select(e => new KeyValuePair<string, string>(NuvTools.Security.Models.ClaimTypes.Permission, e)).ToList();
            return role;
        }

        public async Task<IResult> IncluirAsync(Role value)
        {
            var existingRole = await _roleManager.FindByNameAsync(value.Name);

            if (existingRole is not null)
                return Result<int>.Fail(Messages.RoleAlreadyExists);

            var response = await _roleManager.CreateAsync(new Role(value.Name));

            return (response.Succeeded) ? Result.Success(Messages.OperationPerformedSuccessfully) :
                                            Result.Fail(Messages.TheOperationCouldNotBePerformed);
        }

        public async Task<IResult> AlterarAsync(int id, Role value)
        {
            var existingRole = await _roleManager.FindByIdAsync(id.ToString());

            existingRole.Name = value.Name;
            existingRole.NormalizedName = value.Name.ToUpper();

            var response = await _roleManager.UpdateAsync(existingRole);

            return (response.Succeeded) ? Result.Success(Messages.OperationPerformedSuccessfully) :
                                            Result.Fail(Messages.TheOperationCouldNotBePerformed);
        }

        public async Task<List<string>> GetPermissionsAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            var claims = (await _roleManager.GetClaimsAsync(role))
                                        .Where(e => e.Type == NuvTools.Security.Models.ClaimTypes.Permission)
                                        .Select(e => e.Value).ToList();

            return claims;
        }

        public async Task<IResult> UpdatePermissionsAsync(int id, List<string> permissionClaims)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role.Name == "Administrator")
                {
                    return Result.Fail($"Não é permitido alterar as permissões desse Perfil.");
                }
                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }

                foreach (var claim in permissionClaims)
                {
                    await _roleManager.AddPermissionClaim(role, claim);
                }

                return Result.Success("Permissões atualizadas");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

    }
}
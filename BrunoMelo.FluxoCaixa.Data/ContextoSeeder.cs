using BrunoMelo.FluxoCaixa.Models.Data.Seguranca;
using BrunoMelo.FluxoCaixa.Models.Security;
using BrunoMelo.FluxoCaixa.Models.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NuvTools.Common.Enums;
using NuvTools.Security.Extensions;
using NuvTools.Security.Identity.AspNetCore.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Data;

public partial class ContextoSeeder
{
    private readonly ILogger<ContextoSeeder> _logger;
    private readonly Contexto _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public ContextoSeeder(Contexto context, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<ContextoSeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    public void Initialize()
    {
        AddPermissions();
        AddAdministrator();
        _context.SaveChanges();
    }

    private void AddPermissions()
    {
        Task.Run(async () =>
        {
            var adminRoleInDb = await _roleManager.FindByNameAsync(Enumeracao.Role.Administrador.GetDescription());
            if (adminRoleInDb == null)
            {
                _logger.LogError("Perfil administrador não encontrado.");
                return;
            }

            var permissions = Permissions.GetAllPermissions();

            var currentPermissions = await _roleManager.GetClaimsAsync(adminRoleInDb);

            var newPermissions = permissions.Where(a => !currentPermissions.Any(b => b.Value == a.Value));

            if(newPermissions.Any())
                await _roleManager.AddClaimsAsync(adminRoleInDb, newPermissions);

            //Verifica perfil usuário
            var perfilUser = await _roleManager.FindByNameAsync(Enumeracao.Role.Usuario.GetDescription());
            if (perfilUser == null)
            {
                _logger.LogError("Perfil usuário não encontrado.");
                return;
            }

            List<Claim> permissionsParticipant = new ();
            permissionsParticipant.AddPermissionByClass(typeof(Permissions.Conta));
            permissionsParticipant.AddPermissionByClass(typeof(Permissions.Transacao));

            var currentPermissionsUser = await _roleManager.GetClaimsAsync(perfilUser);
            var newPermissionsUser = permissionsParticipant.Where(a => !currentPermissionsUser.Any(b => b.Value == a.Value)).AsEnumerable();

            if(newPermissionsUser.Any())
                await _roleManager.AddClaimsAsync(perfilUser, newPermissionsUser);

        }).GetAwaiter().GetResult();
    }

    private void AddAdministrator()
    {
        Task.Run(async () =>
        {
            //Check if Role Exists
            var adminRoleInDb = await _roleManager.FindByNameAsync(Enumeracao.Role.Administrador.GetDescription());
            if (adminRoleInDb == null)
            {
                _logger.LogError("Perfil administrador não encontrado.");
                return;
            }

            //Check if User Exists
            var superUser = new User
            {
                Name = "Fluxo Caixa",
                Surname = "Administrador",
                Email = "administrador@rfmelo.com",
                UserName = "administrador@rfmelo.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Status = true,
                SistemaId = (short)Enumeracao.TipoUsuarioSistema.Sistema
            };
            var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
            if (superUserInDb == null)
            {
                await _userManager.CreateAsync(superUser, "fluxocaixa@2022");
                var result = await _userManager.AddToRoleAsync(superUser, Enumeracao.Role.Administrador.GetDescription());
                if (result.Succeeded)
                {
                    
                }
                _logger.LogInformation("Seeded User with Administrator Role.");
            }
        }).GetAwaiter().GetResult();
    }
}
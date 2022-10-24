using BrunoMelo.FluxoCaixa.Models.Data.Seguranca;
using BrunoMelo.FluxoCaixa.Models.Util;
using Microsoft.AspNetCore.Identity;
using NuvTools.Common.Enums;
using NuvTools.Common.Mail;
using NuvTools.Common.ResultWrapper;
using NuvTools.Resources;
using NuvTools.Security.Identity.AspNetCore.Services;
using System.Linq;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Services.Security;

public class UserService : UserServiceBase<User, Role, int>
{
    public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, SMTPMailService mailService) : base(userManager, roleManager, mailService)
    {
    }

    public async Task<IResult> CreateAsync(User request, string origin)
    {
        return await CreateAndSendConfirmationEmailAsync(request, origin, Enumeracao.Role.Usuario.GetDescription());
    }

    public async Task<NuvTools.Security.Identity.Models.Form.UserForm> GetUserForClientAsync(int id)
    {
        var e = await GetAsync(id);

        return new NuvTools.Security.Identity.Models.Form.UserForm { Id = e.Id, Name = e.Name, Surname = e.Surname, Email = e.Email, Status = e.Status };
    }

    public IQueryable<NuvTools.Security.Identity.Models.Form.UserForm> GetAllUserForClientAsync()
    {
        return GetAllAsync().Select(e => new NuvTools.Security.Identity.Models.Form.UserForm { Id = e.Id, Name = e.Name, Surname = e.Surname, Email = e.Email, Status = e.Status });
    }

    public override async Task<IResult> ToggleUserStatusAsync(int id)
    {
        var user = await GetAsync(id);

        if (user == null)
            return Result.Fail(Messages.UserNotFound);

        var IsAdmin = await _userManager.IsInRoleAsync(user, Enumeracao.Role.Administrador.GetDescription());
        if (IsAdmin)
        {
            return Result.Fail("Administrators Profile's Status cannot be toggled");
        }

        await base.ToggleUserStatusAsync(id);

        return Result.Success();
    }
}
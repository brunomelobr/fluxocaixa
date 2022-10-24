using BrunoMelo.FluxoCaixa.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuvTools.Security.AspNetCore.Services;
using NuvTools.Security.Identity.Models.Form;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.API.Controllers.Security;

[Authorize]
[Route("security/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserService _accountService;
    private readonly CurrentUserService _currentUser;

    public AccountController(UserService accountService, CurrentUserService currentUser)
    {
        _accountService = accountService;
        _currentUser = currentUser;
    }


    [HttpPost("updateprofile")]
    public async Task<ActionResult> UpdateProfile(UpdateProfileForm value)
    {
        var response = await _accountService.UpdateAsync(new Models.Data.Seguranca.User
        {
            Id = int.Parse(_currentUser.NameIdentifier),
            Name = value.Name,
            Surname = value.Surname
        });

        return Ok(response);
    }

    [HttpPut("changepassword")]
    public async Task<ActionResult> ChangePassword(ChangePasswordForm model)
    {
        var response = await _accountService.ChangePasswordAsync(int.Parse(_currentUser.NameIdentifier), model);
        return Ok(response);
    }

}
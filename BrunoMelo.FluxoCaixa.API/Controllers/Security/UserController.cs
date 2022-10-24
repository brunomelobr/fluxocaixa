using BrunoMelo.FluxoCaixa.Models.Security;
using BrunoMelo.FluxoCaixa.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuvTools.Common.Numbers;
using NuvTools.Common.ResultWrapper;
using NuvTools.Security.Identity.Models;
using NuvTools.Security.Identity.Models.Form;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.API.Controllers.Security;

[Authorize]
[Route("security/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    [Authorize(Policy = Permissions.Users.View)]
    [HttpGet]
    public IQueryable<UserForm> GetAll()
    {
        return _service.GetAllUserForClientAsync();
    }

    [Authorize(Policy = Permissions.Users.View)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserForm>> GetById(int id)
    {
        return await _service.GetUserForClientAsync(id);
    }

    [Authorize(Policy = Permissions.Users.View)]
    [HttpGet("{id}/roles")]
    public async Task<ActionResult<IList<string>>> GetRolesAsync(int id)
    {
        return Ok(await _service.GetRolesAsync(id));
    }

    [Authorize(Policy = Permissions.Users.Edit)]
    [HttpPut("{id}/roles")]
    public async Task<ActionResult<IResult>> UpdateRolesAsync(UserRoles value)
    {
        return Ok(await _service.UpdateRolesAsync(value));
    }

    [AllowAnonymous]
    [HttpPost("{id?}")]
    public async Task<ActionResult<IResult<int>>> SaveAsync(UserWithPasswordForm value, int? id)
    {
        var origin = Request.Headers["origin"];

        var userDado = new Models.Data.Seguranca.User
        {
            Id = value.Id,
            Name = value.Name,
            Surname = value.Surname,
            Email = value.Email,
            EmailConfirmed = value.EmailConfirmed,
            Password = value.Password,
            Status = value.Status
        };

        return Ok(id == null ?
                      await _service.CreateAsync(userDado, origin)
                    : await _service.UpdateAsync(userDado));
    }

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ActionResult<IResult>> DeleteAsync(int id)
    {
        return Ok(await _service.DeleteAsync(id));
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<ActionResult<IResult<string>>> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
    {
        return Ok(await _service.ConfirmEmailAsync(userId.ParseToIntOrNull(true).Value, code));
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<ActionResult<IResult>> ToggleUserStatusAsync(int id)
    {
        return Ok(await _service.ToggleUserStatusAsync(id));
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<ActionResult<IResult>> ForgotPasswordAsync(ForgotPasswordForm request)
    {
        var origin = Request.Headers["origin"];
        return Ok(await _service.RequestResetPasswordAsync(request.Email, origin));
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<ActionResult<IResult>> ResetPasswordAsync(ResetPasswordForm request)
    {
        return Ok(await _service.ResetPasswordAsync(request));
    }
}
using BrunoMelo.FluxoCaixa.Services.Security;
using Microsoft.AspNetCore.Mvc;
using NuvTools.Security.Identity.Models.Api;
using NuvTools.Security.Identity.Models.Form;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.API.Controllers.Security;

[Route("api/identity/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IdentityService _identityService;

    public TokenController(IdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<ActionResult<TokenResponse>> Get(LoginForm model)
    {
        var response = await _identityService.LoginAsync(model);

        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequest model)
    {
        var response = await _identityService.GetRefreshTokenAsync(model);
        return Ok(response);
    }
}
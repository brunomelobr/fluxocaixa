using BrunoMelo.FluxoCaixa.Models.Data.Seguranca;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NuvTools.Common.Configuration;
using NuvTools.Common.ResultWrapper;
using NuvTools.Security.Identity.Models.Api;
using NuvTools.Security.Identity.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Services.Security;
public class IdentityService
{
    private readonly UserManager<Models.Data.Seguranca.User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ConfigurationSection _appConfig;

    public IdentityService(
        UserManager<Models.Data.Seguranca.User> userManager, RoleManager<Role> roleManager,
        IOptions<ConfigurationSection> appConfig)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _appConfig = appConfig.Value;
    }

    public async Task<IResult<TokenResponse>> LoginAsync(LoginForm model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Result<TokenResponse>.Fail("Usuário não encontrado.");
        }
        if (!user.Status)
        {
            return Result<TokenResponse>.Fail("Usuário inativado. Por favor, entre em contato com o adiministrador do sistema.");
        }
        if (!user.EmailConfirmed)
        {
            return Result<TokenResponse>.Fail("O e-mail não foi confirmado.");
        }
        var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordValid)
        {
            return Result<TokenResponse>.Fail("Credenciais inválidas.");
        }

        user.RefreshToken = NuvTools.Security.Util.TokenHelper.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        var token = await GenerateJwtAsync(user);
        var response = new TokenResponse { Token = token, RefreshToken = user.RefreshToken };
        return Result<TokenResponse>.Success(response);
    }

    public async Task<IResult<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model)
    {
        if (model is null)
        {
            return Result<TokenResponse>.Fail("Token inválido.");
        }
        var userPrincipal = NuvTools.Security.Util.TokenHelper.GetPrincipalFromExpiredToken(model.Token, _appConfig.Security.SecretKey);
        var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
            return Result<TokenResponse>.Fail("Usuário não encontrado.");
        if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return Result<TokenResponse>.Fail("Token inválido.");

        var token = await GenerateJwtAsync(user);

        user.RefreshToken = NuvTools.Security.Util.TokenHelper.GenerateRefreshToken();
        await _userManager.UpdateAsync(user);

        var response = new TokenResponse { Token = token, RefreshToken = user.RefreshToken, RefreshTokenExpiryTime = user.RefreshTokenExpiryTime };
        return Result<TokenResponse>.Success(response);
    }

    private async Task<string> GenerateJwtAsync(Models.Data.Seguranca.User user)
    {
        return NuvTools.Security.Util.TokenHelper.Generate(_appConfig.Security.SecretKey,
                                _appConfig.Security.Issuer,
                                _appConfig.Security.Audience,
                                await GetClaimsAsync(user),
                                DateTime.UtcNow.AddDays(2));
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(Models.Data.Seguranca.User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();
        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            var thisRole = await _roleManager.FindByNameAsync(roles[i]);
            var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);
            foreach (var permission in allPermissionsForThisRoles)
            {
                permissionClaims.Add(permission);
            }
        }

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
            }
        .Union(userClaims)
        .Union(roleClaims)
        .Union(permissionClaims);

        return claims;
    }

}
using Blazored.LocalStorage;
using BrunoMelo.FluxoCaixa.Client.Web.Infrastructure.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using NuvTools.Common.ResultWrapper;
using NuvTools.Security.Identity.Models.Api;
using NuvTools.Security.Identity.Models.Form;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Services.Security;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthenticationService(
        HttpClient httpClient,
        ILocalStorageService localStorage,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<ClaimsPrincipal> CurrentUser()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User;
    }

    public async Task<IResult> Login(LoginForm model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/identity/token", model);
        var result = await response.ToResult<TokenResponse>();
        if (result.Succeeded)
        {
            var token = result.Data.Token;
            var refreshToken = result.Data.RefreshToken;
            await _localStorage.SetItemAsync("authToken", token);
            await _localStorage.SetItemAsync("refreshToken", refreshToken);
            
            ((ApplicationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(model.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return Result.Success();
        }
        else
        {
            return Result.Fail(result.Messages);
        }
    }

    public async Task<IResult> Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");
        await _localStorage.RemoveItemAsync("userImageURL");            
        ((ApplicationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
        
        return Result.Success();
    }

    public async Task<string> RefreshToken()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

        var tokenRequest = JsonSerializer.Serialize(new TokenResponse { Token = token, RefreshToken = refreshToken });
        var bodyContent = new StringContent(tokenRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/identity/token/refresh", bodyContent);

        var result = await response.ToResult<TokenResponse>();

        if (!result.Succeeded)
        {
            throw new ApplicationException($"Something went wrong during the refresh token action");
        }

        token = result.Data.Token;
        refreshToken = result.Data.RefreshToken;
        await _localStorage.SetItemAsync("authToken", token);
        await _localStorage.SetItemAsync("refreshToken", refreshToken);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return token;
    }

    public async Task<string> TryRefreshToken()
    {
        //check if token exists
        var availableToken = await _localStorage.GetItemAsync<string>("refreshToken");
        if (string.IsNullOrEmpty(availableToken)) return string.Empty;
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var exp = user.FindFirst(c => c.Type.Equals("exp")).Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var timeUTC = DateTime.UtcNow;
        var diff = expTime - timeUTC;
        if (diff.TotalMinutes <= 1)
            return await RefreshToken();
        return string.Empty;
    }

    public async Task<string> TryForceRefreshToken()
    {
        return await RefreshToken();
    }
}
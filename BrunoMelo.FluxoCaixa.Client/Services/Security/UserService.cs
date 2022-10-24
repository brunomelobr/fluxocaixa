using NuvTools.Common.ResultWrapper;
using NuvTools.Security.Identity.Models;
using NuvTools.Security.Identity.Models.Form;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Services.Security;

public class UserService
{
    private const string Base = "security/user";
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserForm>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<UserForm>>(Base);
    }

    public async Task<UserForm> GetAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<UserForm>($"{Base}/{id}");
    }

    public async Task<IResult<int>> SaveAsync(UserWithPasswordForm value)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Base}/{(value.Id > 0 ? value.Id : null)}", value);
        return await response.ToResult<int>();
    }

    public async Task<IResult> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{Base}/{id}");
        return await response.ToResult();
    }

    public async Task<IResult> ToggleUserStatusAsync(int id)
    {
        var response = await _httpClient.PutAsync($"{Base}/{id}/toggle-status", null);
        return await response.ToResult();
    }

    public async Task<List<string>> GetRolesAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<List<string>>($"{Base}/{id}/roles");
    }

    public async Task<IResult> UpdateRolesAsync(UserRoles value)
    {
        var response = await _httpClient.PutAsJsonAsync($"{Base}/{value.UserId}/roles", value);
        return await response.ToResult();
    }

    public async Task<IResult> ForgotPasswordAsync(ForgotPasswordForm value)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Base}/forgot-password", value);
        return await response.ToResult();
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordForm value)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Base}/reset-password", value);
        return await response.ToResult();
    }

    public async Task<IResult> ConfirmEmailAsync(int id, string code)
    {
        var response = await _httpClient.GetAsync($"{Base}/confirm-email?userId={id}&code={code}");
        return await response.ToResult();
    }
}
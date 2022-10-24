using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NuvTools.Common.ResultWrapper;
using BrunoMelo.FluxoCaixa.Models.Data.Seguranca;

namespace BrunoMelo.FluxoCaixa.Client.Services.Security;

public class RoleService
{
    private static readonly string Base = "security/role";
    private readonly HttpClient _httpClient;

    public RoleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult<string>> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{Base}/{id}");
        return await response.ToResult<string>();
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Role>>(Base);
    }

    public async Task<Role> GetWithPermissionsAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Role>($"{Base}/{id}/?permissions=true");
    }

    public async Task<IResult<int>> SaveAsync(Role value)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{Base}/{(value.Id > 0 ? value.Id : null)}", value);
        return await response.ToResult<int>();
    }

    public async Task<List<string>> GetPermissionsAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<List<string>>($"{Base}/{id}/permissions");
    }

    public async Task<IResult> UpdatePermissionsAsync(int id, List<string> value)
    {
        var response = await _httpClient.PutAsJsonAsync($"{Base}/{id}/permissions", value);
        return await response.ToResult();
    }
}
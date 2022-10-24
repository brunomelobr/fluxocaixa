using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NuvTools.Common.ResultWrapper;
using NuvTools.Security.Identity.Models.Form;

namespace BrunoMelo.FluxoCaixa.Client.Services.Security;

public class AccountService
{
    private const string Base = "security/account";
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult> ChangePasswordAsync(ChangePasswordForm model)
    {
        var response = await _httpClient.PutAsJsonAsync($"{Base}/changepassword", model);
        return await response.ToResult();
    }

    public async Task<IResult> UpdateProfileAsync(UpdateProfileForm model)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Base}/updateprofile", model);
        return await response.ToResult();
    }
}
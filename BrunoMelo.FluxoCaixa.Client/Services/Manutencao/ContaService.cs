using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NuvTools.Common.ResultWrapper;
using BrunoMelo.FluxoCaixa.Models.Data.Manutencao;
using System.Collections.Generic;

namespace BrunoMelo.FluxoCaixa.Client.Services.Manutencao;

public class ContaService
{
    private static readonly string Base = "manutencao/conta";
    private readonly HttpClient _httpClient;

    public ContaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Conta> ConsultarAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Conta>($"{Base}/{id}");
    }

    public async Task<IEnumerable<Conta>> ConsultarAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Conta>>($"{Base}");
    }

    public async Task<IResult<int>> SalvarAsync(Conta objeto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Base}/{(objeto.ContaId > 0 ? objeto.ContaId : null)}", objeto);
        return await response.ToResult<int>();
    }

    public async Task<IResult> ExcluirAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{Base}/{id}");
        return await response.ToResult();
    }
}
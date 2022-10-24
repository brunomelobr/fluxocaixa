using BrunoMelo.FluxoCaixa.Models.Data.Manutencao;
using NuvTools.Common.ResultWrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Services.Manutencao;

public class CategoriaService
{
    private static readonly string Base = "manutencao/categoria";
    private readonly HttpClient _httpClient;

    public CategoriaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Categoria>> ConsultarAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Categoria>>(Base); ;
    }

    public async Task<IResult<long>> SalvarAsync(Categoria objeto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{Base}/{(objeto.CategoriaId > 0 ? objeto.CategoriaId : null)}", objeto);
        return await response.ToResult<long>();
    }

    public async Task<IResult> ExcluirAsync(long id)
    {
        var response = await _httpClient.DeleteAsync($"{Base}/{id}");
        return await response.ToResult();
    }
}
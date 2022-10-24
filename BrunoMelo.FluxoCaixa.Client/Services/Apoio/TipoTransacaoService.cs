using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BrunoMelo.FluxoCaixa.Models.Data.Apoio;

namespace BrunoMelo.FluxoCaixa.Client.Services.Apoio;

public class TipoTransacaoService
{
    private static readonly string Base = "apoio/tipotransacao";
    private readonly HttpClient _httpClient;

    public TipoTransacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TipoTransacao>> ConsultarAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<TipoTransacao>>(Base);
    }
}
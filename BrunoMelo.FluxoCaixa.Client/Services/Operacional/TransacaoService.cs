using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NuvTools.Common.ResultWrapper;
using BrunoMelo.FluxoCaixa.Models.Data.Operacional;
using BrunoMelo.FluxoCaixa.Client.Web.Models.Operacional;

namespace BrunoMelo.FluxoCaixa.Client.Services.Operacional;

public class TransacaoService
{
    private const string Base = "operacional/transacao";
    private readonly HttpClient _httpClient;

    public TransacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Transacao> ConsultarAsync(long id)
    {
        return await _httpClient.GetFromJsonAsync<Transacao>($"{Base}/{id}");
    }

    public async Task<IEnumerable<TransacaoResumo>> ConsultarResumoPorPeriodoAsync(int? contaId = null, int? mes = null, int? ano = null)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<TransacaoResumo>>($"{Base}?conta={contaId}&ano={ano}&mes={mes}");
    }

    public async Task<IResult<long>> SalvarAsync(Transacao objeto)
    {
        objeto.TipoTransacao = null;
        objeto.Categoria = null;
        objeto.Conta = null;
        var response = await _httpClient.PostAsJsonAsync($"{Base}/{(objeto.TransacaoId > 0 ? objeto.TransacaoId : null)}", objeto);
        return await response.ToResult<long>();
    }

    public async Task<IResult> ExcluirAsync(long id)
    {
        var response = await _httpClient.DeleteAsync($"{Base}/{id}");
        return await response.ToResult();
    }

}
using BrunoMelo.FluxoCaixa.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BrunoMelo.FluxoCaixa.Services.Apoio;

public class TipoTransacaoService
{
    private readonly Contexto _contexto;

    public TipoTransacaoService(Contexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Models.Data.Apoio.TipoTransacao>> ConsultarAsync()
    {
        return await _contexto.TipoTransacao.ToListAsync();
    }
}
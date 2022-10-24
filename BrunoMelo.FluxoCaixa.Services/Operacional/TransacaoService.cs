using BrunoMelo.FluxoCaixa.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NuvTools.Data.EntityFrameworkCore.Extensions;
using NuvTools.Common.ResultWrapper;
using BrunoMelo.FluxoCaixa.Models.Data.Operacional;
using System.Collections.Generic;
using System.Linq;
using BrunoMelo.FluxoCaixa.Client.Web.Models.Operacional;

namespace BrunoMelo.FluxoCaixa.Services.Operacional;

public class TransacaoService
{
    private readonly Contexto _contexto;

    public TransacaoService(Contexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<Transacao> ConsultarAsync(long id)
    {
        return await _contexto.Transacao.FirstOrDefaultAsync(a => a.TransacaoId == id);
    }

    public async Task<List<TransacaoResumo>> ConsultarResumoAsync(string descricao = null)
    {
        return await _contexto.Transacao
                        .Where(a => descricao == null || a.Descricao.ToLower().Contains(descricao.ToLower())).OrderBy(a => a.Data)
                        .Select(e => new TransacaoResumo
                        {
                            TransacaoId = e.TransacaoId,
                            Data = e.Data,
                            Descricao = e.Descricao,
                            TipoTransacaoId = e.TipoTransacaoId,
                            NomeTipoTransacao = e.TipoTransacao.Nome,
                            NomeCategoria = e.Categoria.Nome,
                            NomeConta = e.Conta.Nome,
                            Valor = e.Valor
                        })
                        .ToListAsync();
    }

    public async Task<IResult<long>> IncluirAsync(Transacao value)
    {
        value.Categoria = null;
        value.Conta = null;
        value.TipoTransacao = null;
        return await _contexto.AddAndSaveAsync<Transacao, long>(value);
    }

    public async Task<IResult> AlterarAsync(long id, Transacao value)
    {
        value.Categoria = null;
        value.Conta = null;
        value.TipoTransacao = null;
        return await _contexto.UpdateAndSaveAsync(value, id);
    }

    public async Task<IResult> ExcluirAsync(long id)
    {
        return await _contexto.RemoveAndSaveAsync<Transacao>(id);
    }
}
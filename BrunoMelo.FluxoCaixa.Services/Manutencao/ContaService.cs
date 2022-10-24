using BrunoMelo.FluxoCaixa.Data;
using System.Threading.Tasks;
using NuvTools.Common.ResultWrapper;
using Microsoft.EntityFrameworkCore;
using NuvTools.Data.EntityFrameworkCore.Extensions;
using BrunoMelo.FluxoCaixa.Models.Data.Manutencao;
using System.Collections.Generic;
using System.Linq;

namespace BrunoMelo.FluxoCaixa.Services.Manutencao;

public class ContaService
{
    private readonly Contexto _contexto;

    public ContaService(Contexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<Conta> ConsultarAsync(int id)
    {
        return await _contexto.Conta.FirstOrDefaultAsync(b => b.ContaId == id);
    }

    public async Task<List<Conta>> ConsultarAsync(string nome = null)
    {
        return await _contexto.Conta.Where(a => nome == null || a.Nome.ToLower().Contains(nome.ToLower())).OrderBy(a => a.Nome).ToListAsync();
    }

    public async Task<IResult<int>> IncluirAsync(Conta objeto)
    {
        return await _contexto.AddAndSaveAsync<Conta, int>(objeto);
    }

    public async Task<IResult> AlterarAsync(int id, Conta objeto)
    {
        return await _contexto.UpdateAndSaveAsync(objeto, id);
    }

    public async Task<IResult> ExcluirAsync(int id)
    {
        return await _contexto.RemoveAndSaveAsync<Conta>(id);            
    }
}
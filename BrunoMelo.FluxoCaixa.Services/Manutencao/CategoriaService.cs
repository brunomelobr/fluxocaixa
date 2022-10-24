using BrunoMelo.FluxoCaixa.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BrunoMelo.FluxoCaixa.Models.Data.Manutencao;
using NuvTools.Common.ResultWrapper;
using NuvTools.Data.EntityFrameworkCore.Extensions;

namespace BrunoMelo.FluxoCaixa.Services.Manutencao;

public class CategoriaService
{
    private readonly Contexto _contexto;

    public CategoriaService(Contexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<Categoria> ConsultarAsync(int id)
    {
        return await _contexto.Categoria.FirstOrDefaultAsync(b => b.CategoriaId == id);
    }

    public async Task<List<Categoria>> ConsultarAsync(string nome = null)
    {
        return await _contexto.Categoria.Where(a => nome == null || a.Nome.ToLower().Contains(nome.ToLower())).OrderBy(a => a.Nome).ToListAsync();
    }

    public async Task<IResult<int>> IncluirAsync(Categoria objeto)
    {
        return await _contexto.AddAndSaveAsync<Categoria, int>(objeto);
    }

    public async Task<IResult> AlterarAsync(int id, Categoria objeto)
    {
        return await _contexto.UpdateAndSaveAsync(objeto, id);
    }

    public async Task<IResult> ExcluirAsync(int id)
    {
        return await _contexto.RemoveAndSaveAsync<Categoria>(id);
    }
}
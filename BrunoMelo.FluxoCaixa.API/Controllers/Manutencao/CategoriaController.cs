using BrunoMelo.FluxoCaixa.Models.Data.Manutencao;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BrunoMelo.FluxoCaixa.Services.Manutencao;
using NuvTools.Common.ResultWrapper;
using BrunoMelo.FluxoCaixa.Data;
using System.Collections.Generic;

namespace BrunoMelo.FluxoCaixa.API.Controllers.Manutencao;

[ApiController]
[Route("manutencao/categoria")]
public class CategoriaController : ControllerBase
{
    private readonly CategoriaService _service;

    public CategoriaController(CategoriaService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<Categoria> ConsultarAsync(int id)
    {
        return await _service.ConsultarAsync(id);
    }

    [HttpGet]
    public async Task<List<Categoria>> ConsultarAsync()
    {
        return await _service.ConsultarAsync();
    }

    [HttpPost("{id?}")]
    public async Task<ActionResult<IResult<int>>> SalvarAsync([FromBody] Categoria objeto, int? id = null)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != null && id != objeto.CategoriaId)
        {
            return BadRequest();
        }

        return Ok(id == null ?
                      await _service.IncluirAsync(objeto)
                    : await _service.AlterarAsync(id.Value, objeto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<IResult>> ExcluirAsync(int id)
    {
        return Ok(await _service.ExcluirAsync(id));
    }
}

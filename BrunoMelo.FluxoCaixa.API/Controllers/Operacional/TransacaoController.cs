using BrunoMelo.FluxoCaixa.Models.Data.Operacional;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BrunoMelo.FluxoCaixa.Services.Operacional;
using NuvTools.Common.ResultWrapper;
using System.Collections.Generic;
using BrunoMelo.FluxoCaixa.Client.Web.Models.Operacional;
using Microsoft.AspNetCore.Authorization;

namespace BrunoMelo.FluxoCaixa.API.Controllers.Operacional;

[Authorize]
[ApiController]
[Route("operacional/transacao")]
public class TransacaoController : ControllerBase
{
    private readonly TransacaoService _servico;

    public TransacaoController(TransacaoService dimensionamentoDadoPessoalService)
    {
        _servico = dimensionamentoDadoPessoalService;
    }

    [HttpGet("{id}")]
    public async Task<Transacao> ConsultarAsync(long id)
    {
        return await _servico.ConsultarAsync(id);
    }

    [HttpGet]
    public async Task<List<TransacaoResumo>> ConsultarAsync()
    {
        return await _servico.ConsultarResumoAsync();
    }

    [HttpPost("{id?}")]
    public async Task<ActionResult<IResult<long>>> SalvarAsync(Transacao value, long? id = null)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != null && id != value.TransacaoId)
        {
            return BadRequest();
        }

        return Ok(id == null ?
                      await _servico.IncluirAsync(value)
                    : await _servico.AlterarAsync(id.Value, value));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Excluir(long id)
    {
        return Ok(await _servico.ExcluirAsync(id));
    }
}

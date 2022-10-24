using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BrunoMelo.FluxoCaixa.Models.Data.Apoio;
using System.Collections.Generic;
using BrunoMelo.FluxoCaixa.Services.Apoio;

namespace BrunoMelo.FluxoCaixa.API.Controllers.Apoio;

[ApiController]
[Route("apoio/tipotransacao")]
public class TipoTransacaoController : ControllerBase
{
    private readonly TipoTransacaoService _service;

    public TipoTransacaoController(TipoTransacaoService service)
    {
        _service = service;
    }

    [HttpGet()]
    public async Task<IEnumerable<TipoTransacao>> ConsultarAsync()
    {
        return await _service.ConsultarAsync();
    }
}

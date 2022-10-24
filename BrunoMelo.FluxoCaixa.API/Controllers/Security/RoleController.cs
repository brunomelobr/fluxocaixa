using BrunoMelo.FluxoCaixa.Models.Data.Seguranca;
using BrunoMelo.FluxoCaixa.Models.Security;
using BrunoMelo.FluxoCaixa.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuvTools.Common.ResultWrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.API.Controllers.Security;

[Route("security/role")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleService _service;

    public RoleController(RoleService service)
    {
        _service = service;
    }

    [Authorize(Policy = Permissions.Roles.View)]
    [HttpGet]
    public IQueryable<Role> GetAll()
    {
        return _service.GetAllAsync();
    }

    [Authorize(Policy = Permissions.Roles.View)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetWithPermissionAsync(int id, bool permissions = false)
    {
        return Ok(permissions ? await _service.GetWithPermissionAsync(id) : await _service.GetAsync(id));
    }

    [Authorize(Policy = Permissions.Roles.Create)]
    [HttpPost("{id?}")]
    public async Task<ActionResult<IResult>> Salvar(Role value, int? id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != null && id != value.Id)
        {
            return BadRequest();
        }

        return Ok(id == null ?
                      await _service.IncluirAsync(value)
                    : await _service.AlterarAsync(id.Value, value));
    }

    [Authorize(Policy = Permissions.Roles.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);
        return Ok(response);
    }

    [Authorize(Policy = Permissions.Roles.Edit)]
    [HttpGet("{id}/permissions")]
    public async Task<ActionResult<List<string>>> GetPermissions([FromRoute] int id)
    {
        var response = await _service.GetPermissionsAsync(id);
        return Ok(response);
    }

    [Authorize(Policy = Permissions.Roles.Edit)]
    [HttpPut("{id}/permissions")]
    public async Task<ActionResult<IResult>> Update(int id, List<string> claims)
    {
        var response = await _service.UpdatePermissionsAsync(id, claims);
        return Ok(response);
    }
}
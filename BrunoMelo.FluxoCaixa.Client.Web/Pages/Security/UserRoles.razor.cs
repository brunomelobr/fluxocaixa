using BrunoMelo.FluxoCaixa.Client.Services.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NuvTools.Common.Numbers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security;

public partial class UserRoles
{
    [Inject] UserService UserService { get; set; }
    [Inject] RoleService RoleService { get; set; }

    public class ModelView
    {
        public FluxoCaixa.Models.Data.Seguranca.Role Data { get; set; }
        public bool Selected { get; set; }
    }

    [Parameter]
    public string Id { get; set; }

    public List<ModelView> ListModelView { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var result = await UserService.GetAsync(Id.ParseToIntOrNull(true).Value);

        if (result == null) return;

        ListModelView = (await RoleService.GetAllAsync()).Select(e => new ModelView { Data = e }).ToList();

        var userRoles = await UserService.GetRolesAsync(result.Id);

        foreach (var item in ListModelView)
        {
            item.Selected = userRoles.Any(e => e == item.Data.Name);
        }
    }

    private async Task SaveAsync()
    {
        var request = new NuvTools.Security.Identity.Models.UserRoles()
        {
            UserId = Id.ParseToIntOrNull(true).Value,
            Roles = ListModelView.Where(e => e.Selected).Select(e => e.Data.Name).ToList()
        };

        var result = await UserService.UpdateRolesAsync(request);

        if (result.Succeeded)
        {
            _snackBar.Add(_localizer[result.Messages[0]], Severity.Success);
            _navigationManager.NavigateTo(Routes.Security.Users);
        }
        else
        {
            foreach (var error in result.Messages)
            {
                _snackBar.Add(_localizer[error], Severity.Error);
            }
        }
    }

    private void GoBack()
    {
        _navigationManager.NavigateTo(Routes.Security.Users);
    }
}
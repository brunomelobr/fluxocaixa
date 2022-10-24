using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using BrunoMelo.FluxoCaixa.Models.Security;
using System.Collections.Generic;
using System.Linq;
using MudBlazor;
using NuvTools.Common.Numbers;
using BrunoMelo.FluxoCaixa.Client.Services.Security;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security;

public partial class RolePermissions
{
    [Inject] RoleService Service { get; set; }

    [Parameter]
    public string Id { get; set; }

    public class ModelView
    {
        public string Data { get; set; }
        public bool Selected { get; set; }
    }

    private List<ModelView> ListModelView { get; set; }

    private FluxoCaixa.Models.Data.Seguranca.Role ObjetoEdicao { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //TODO: Fazer tratamento quando nulo ou quando não for encontrado
        ObjetoEdicao = await Service.GetWithPermissionsAsync(Id.ParseToIntOrNull().Value);

        var allPermissions = Permissions.GetAllPermissions();

        ListModelView = allPermissions.Select(e => new ModelView { Data = e.Value, Selected = ObjetoEdicao.Claims.Any(p => p.Value == e.Value) }).ToList();
    }

    private void GoBack()
    {
        _navigationManager.NavigateTo(Routes.Security.Roles);
    }

    private async Task SaveAsync()
    {
        var result = await Service.UpdatePermissionsAsync(Id.ParseToIntOrNull().Value, ListModelView.Where(e => e.Selected).Select(e => e.Data).ToList());
        if (result.Succeeded)
        {
            _snackBar.Add(_localizer[result.Messages[0]], Severity.Success);
            _navigationManager.NavigateTo(Routes.Security.Roles);
        }
        else
        {
            foreach (var error in result.Messages)
            {
                _snackBar.Add(_localizer[error], Severity.Error);
            }
        }
    }
}
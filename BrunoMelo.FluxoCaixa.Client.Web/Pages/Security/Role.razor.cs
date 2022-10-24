using BrunoMelo.FluxoCaixa.Client.Services.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security;

public partial class Role
{
    [Inject] RoleService Service { get; set; }

    public List<FluxoCaixa.Models.Data.Seguranca.Role> ListModelView = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadMainListAsync();
    }

    private async Task LoadMainListAsync()
    {
        ListModelView = await Service.GetAllAsync();
    }

    private async Task InvokeModal(FluxoCaixa.Models.Data.Seguranca.Role item = null)
    {
        var parameters = new DialogParameters();

        if (item != null)
        {
            parameters.Add(nameof(item.Id), item.Id);
            parameters.Add(nameof(item.Name), item.Name);
        }

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
        var dialog = _dialogService.Show<RoleEditModal>("Modal", parameters, options);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await LoadMainListAsync();
        }
    }

    private void ManagePermissions(int roleId)
    {
        _navigationManager.NavigateTo(Routes.Security.GetPermissions(roleId));
    }
}
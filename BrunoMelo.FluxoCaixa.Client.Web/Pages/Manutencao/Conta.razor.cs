using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;
using BrunoMelo.FluxoCaixa.Client.Services.Manutencao;
using Microsoft.AspNetCore.Components;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Manutencao;

public partial class Conta
{

    [Inject] ContaService Service { get; set; }

    public class ModelView
    {
        public FluxoCaixa.Models.Data.Manutencao.Conta Data { get; set; }
    }

    private List<ModelView> ListModelView { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (!(await _stateProvider.GetAuthenticationStateProviderUserAsync()).Identity.IsAuthenticated)
        {
            _navigationManager.NavigateTo("/login");
            return;
        }

        await LoadMainListAsync();
    }

    private async Task LoadMainListAsync()
    {
        var resultado = await Service.ConsultarAsync();

        if (resultado != null)
            ListModelView = resultado.Select(a =>
                            new ModelView
                            {
                                Data = a
                            }).ToList();
    }

    private async Task InvokeModalAsync(int? Id = null)
    {
        var parameters = new DialogParameters();

        if (Id != null)
            parameters.Add(nameof(Id), Id);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
        var dialog = _dialogService.Show<ContaEdicaoModal>("Modal", parameters, options);
        var result = await dialog.Result;
        
        if (!result.Cancelled)
        {
            await LoadMainListAsync();
        }
    }
}
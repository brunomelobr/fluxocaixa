using BrunoMelo.FluxoCaixa.Client.Web.Models.Operacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;
using Microsoft.AspNetCore.Components;
using BrunoMelo.FluxoCaixa.Client.Services.Operacional;
using NuvTools.Common.Enums;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Operacional;

public partial class Transacao
{
    [Inject] TransacaoService Service { get; set; }

    public class ModelView
    {
        public TransacaoResumo Data { get; set; }
        public bool Exibir { get; set; }
    }

    private TableGroupDefinition<ModelView> _groupDefinition = new()
    {
        GroupName = "Data",
        Indentation = false,
        Expandable = false,
        Selector = (e) => e.Data.Data
    };

    private List<ModelView> ListModelView { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        if (!(await _stateProvider.GetAuthenticationStateProviderUserAsync()).Identity.IsAuthenticated)
        {
            _navigationManager.NavigateTo("/login");
            return;
        }

        await LoadMainListAsync();
    }

    private decimal Totalizar(IEnumerable<ModelView> itens) {
        return itens.Sum(e => e.Data.TipoTransacaoId == (short)FluxoCaixa.Models.Util.Enumeracao.TipoTransacao.Debito ? e.Data.Valor * -1 : e.Data.Valor);
    }

    private async Task LoadMainListAsync()
    {
        var resultado = await Service.ConsultarResumoPorPeriodoAsync();

        if (resultado != null)
            ListModelView = resultado.Select(a =>
                            new ModelView
                            {
                                Data = a
                            }).ToList();
    }

    private async Task InvokeModalAsync(long? Id = null)
    {
        var parameters = new DialogParameters();

        if (Id != null)
            parameters.Add(nameof(Id), Id);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
        var dialog = _dialogService.Show<TransacaoEdicaoModal>("Modal", parameters, options);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await LoadMainListAsync();
        }
    }

}
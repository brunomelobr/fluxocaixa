using BrunoMelo.FluxoCaixa.Client.Services.Manutencao;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using NuvTools.Resources;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Manutencao;

public partial class ContaEdicaoModal
{
    [Inject] ContaService Service { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [Parameter] public int? Id { get; set; }

    public EditContext EditContext { get; set; }
    public FluxoCaixa.Models.Data.Manutencao.Conta ObjetoEdicao = new();

    private bool ExibeConfirmacaoExclusao { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        EditContext = new(ObjetoEdicao);

        if (!(await _stateProvider.GetAuthenticationStateProviderUserAsync()).Identity.IsAuthenticated)
        {
            _navigationManager.NavigateTo("/login");
            return;
        }

        if (Id != null && Id.HasValue)
        {
            ObjetoEdicao = await Service.ConsultarAsync(Id.Value);
            EditContext = new(ObjetoEdicao);
        }
    }

    public void Cancelar()
    {
        MudDialog.Cancel();
    }

    private async Task ExcluirAsync()
    {
        ExibirMensagemResultado(await Service.ExcluirAsync(ObjetoEdicao.ContaId));
    }

    private async Task SalvarAsync()
    {
        if (!EditContext.Validate()) return;

        ExibirMensagemResultado(await Service.SalvarAsync(ObjetoEdicao));
    }

    private void ExibirMensagemResultado(NuvTools.Common.ResultWrapper.IResult response)
    {
        if (response.Succeeded)
        {
            _snackBar.Add(_localizerMessages[nameof(Messages.OperationPerformedSuccessfully)], Severity.Success);
            MudDialog.Close();
        }
        else
        {
            string erro = _localizerMessages[nameof(Messages.TheOperationCouldNotBePerformed)];

            foreach (var message in response.Messages)
            {
                erro += "\n" + message;
            }

            _snackBar.Add(erro, Severity.Error);
        }
    }
}
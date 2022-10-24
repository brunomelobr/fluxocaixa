using BrunoMelo.FluxoCaixa.Client.Services.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using NuvTools.Resources;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security;

public partial class RoleEditModal
{
    [Inject] RoleService Service { get; set; }

    [Parameter]
    public int Id { get; set; }

    [Parameter]
    public string Name { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    private EditContext EditContext { get; set; }
    public FluxoCaixa.Models.Data.Seguranca.Role ObjetoEdicao = new();

    private bool ExibeConfirmacaoExclusao { get; set; } = false;

    protected override void OnInitialized()
    {
        ObjetoEdicao = new FluxoCaixa.Models.Data.Seguranca.Role { Id = Id, Name = Name };
        EditContext = new(ObjetoEdicao);
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task DeleteAsync()
    {
        ExibirMensagemResultado(await Service.DeleteAsync(Id));
    }

    private async Task SaveAsync()
    {
        if (!EditContext.Validate()) return;

        ExibirMensagemResultado(await Service.SaveAsync(ObjetoEdicao));
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
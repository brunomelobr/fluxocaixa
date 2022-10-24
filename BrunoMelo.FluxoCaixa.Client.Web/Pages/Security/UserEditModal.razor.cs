using BrunoMelo.FluxoCaixa.Client.Services.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using NuvTools.Resources;
using NuvTools.Security.Identity.Models.Form;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security
{
    public partial class UserEditModal
    {
        [Inject] UserService service { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private EditContext EditContext { get; set; }
        public UserWithPasswordForm ObjetoEdicao = new();

        protected override void OnInitialized()
        {
            EditContext = new(ObjetoEdicao);
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            if (!EditContext.Validate()) return;

            ExibirMensagemResultado(await service.SaveAsync(ObjetoEdicao));
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

        private bool PasswordVisibility;
        private InputType PasswordInput = InputType.Password;
        private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
        {
            if (PasswordVisibility)
            {
                PasswordVisibility = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                PasswordVisibility = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

    }
}
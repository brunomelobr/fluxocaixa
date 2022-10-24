using BrunoMelo.FluxoCaixa.Client.Services.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NuvTools.Common.Numbers;
using NuvTools.Resources;
using NuvTools.Security.Identity.Models.Form;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security
{
    public partial class UserProfile
    {
        [Inject] UserService Service { get; set; }

        [Parameter]
        public string Id { get; set; }

        public int IdNumber { get { return Id.ParseToIntOrNull(true).Value; } }

        private char FirstLetterOfName { get; set; }

        public UserForm ObjetoEdicao = new();
        private bool ExibeConfirmacaoExclusao { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            ObjetoEdicao = await Service.GetAsync(IdNumber);

            if (ObjetoEdicao.Name.Length > 0)
            {
                FirstLetterOfName = ObjetoEdicao.Name[0];
            }
        }

        private async Task ToggleUserStatus()
        {
            var result = await Service.ToggleUserStatusAsync(IdNumber);
            if (result.Succeeded)
            {
                _snackBar.Add(_localizer["Updated User Status."], Severity.Success);
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    _snackBar.Add(_localizer[error], Severity.Error);
                }
            }
        }

        private async Task DeleteAsync()
        {
            var result = await Service.DeleteAsync(IdNumber);

            if (result.Succeeded)
            {
                _snackBar.Add(_localizerMessages[nameof(Messages.OperationPerformedSuccessfully)], Severity.Success);
                _navigationManager.NavigateTo(Routes.Security.Users);
            }
            else
            {
                string erro = _localizerMessages[nameof(Messages.TheOperationCouldNotBePerformed)];

                foreach (var message in result.Messages)
                {
                    erro += "\n" + message;
                }

                _snackBar.Add(erro, Severity.Error);
            }
        }

        private void GoBack()
        {
            _navigationManager.NavigateTo(Routes.Security.Users);
        }

    }
}
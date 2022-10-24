using BrunoMelo.FluxoCaixa.Client.Web.Extensions;
using MudBlazor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using NuvTools.Security.Identity.Models.Form;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security
{
    public partial class Account
    {
        private char FirstLetterOfName { get; set; }

        private readonly UpdateProfileForm profileModel = new();
        public int UserId { get; set; }

        private EditContext EditContext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            EditContext = new(profileModel);
            await LoadDataAsync();
        }

        private async Task UpdateProfileAsync()
        {
            var response = await _accountManager.UpdateProfileAsync(profileModel);
            
            if (response.Succeeded)
            {
                _snackBar.Add(_localizer["Conta atualizada com sucesso."], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(_localizer[message], Severity.Error);
                }
            }
        }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            profileModel.Email = user.GetEmail();
            profileModel.Name = user.GetName();
            profileModel.Surname = user.GetSurname();
            UserId = user.GetUserId();

            EditContext = new(profileModel);

            if (profileModel.Name.Length > 0)
            {
                FirstLetterOfName = profileModel.Name[0];
            }
        }
    }
}
using BrunoMelo.FluxoCaixa.Client.Web.Extensions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Shared.Components
{
    public partial class UserCard
    {
        [Parameter] public string Class { get; set; }
        private string Name { get; set; }
        private string Surname { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;

            Email = user.GetEmail();
            Name = user.GetName();
            Surname = user.GetSurname();
            if (Name.Length > 0)
            {
                FirstLetterOfName = Name[0];
            }
        }
    }
}
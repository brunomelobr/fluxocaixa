using BrunoMelo.FluxoCaixa.Client.Web.Extensions;
using MudBlazor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NuvTools.Common.Enums;

namespace BrunoMelo.FluxoCaixa.Client.Web.Shared;

public partial class BarMenu
{
    [Parameter]
    public MudTheme CurrentTheme { get; set; }

    [Parameter]
    public EventCallback<MudTheme> CurrentThemeChanged { get; set; }

    private int CurrentUserId { get; set; }

    private string FirstName { get; set; }
    private string SecondName { get; set; }
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
        if (user == null || !user.Identity.IsAuthenticated) return;

        CurrentUserId = user.GetUserId();
        FirstName = user.GetName();
        if (FirstName.Length > 0)
        {
            FirstLetterOfName = FirstName[0];
        }
    }
    
    private void Logout()
    {
        string logoutConfirmationText = _localizer["DoYouReallyLogout"];
        string logoutText = _localizer["Logout"];
        var parameters = new DialogParameters
        {
            { "ContentText", logoutConfirmationText },
            { "ButtonText", logoutText },
            { "Color", Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

        _dialogService.Show<Dialogs.Logout>("Logout", parameters, options);
    }

    private bool _drawerOpen = true;
    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}
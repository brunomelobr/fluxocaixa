using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using NuvTools.Resources;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NuvTools.Common.Numbers;
using Microsoft.AspNetCore.Components;
using BrunoMelo.FluxoCaixa.Client.Services.Security;
using NuvTools.Security.Identity.Models.Form;


namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Authentication;

public partial class Login
{
    [Inject] UserService UserService { get; set; }

    public EditContext EditContext { get; set; }
    public LoginForm ObjetoEdicao = new();

    [Parameter]
    public int UserId { get; set; }
    [Parameter]
    public string Code { get; set; }
    private bool Wait { get; set; }

    protected override async Task OnInitializedAsync()
    {
        EditContext = new(ObjetoEdicao);

        var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("userId", out var userIdString)
            && QueryHelpers.ParseQuery(uri.Query).TryGetValue("code", out var codeString))
        {
            UserId = userIdString.First().ParseToIntOrNull(true).Value;
            Code = codeString.First();

            var result = await UserService.ConfirmEmailAsync(UserId, Code);

            if (result.Succeeded)
            {
                _snackBar.Add($"{_localizer[result.Messages[0]]} {ObjetoEdicao.Email}.", Severity.Success);
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(_localizer[message], Severity.Error);
                }
            }
        }

        var state = await _stateProvider.GetAuthenticationStateAsync();
        if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
        {
            _navigationManager.NavigateTo("/");
            return;
        }
    }

    private async Task SubmitAsync()
    {
        if (!EditContext.Validate()) return;

        Wait = true;

        var result = await _authenticationManager.Login(ObjetoEdicao);
        if (result.Succeeded)
        {
            _snackBar.Add($"{_localizer[nameof(Fields.Welcome)]} {ObjetoEdicao.Email}.", Severity.Success);
            _navigationManager.NavigateTo("/");
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackBar.Add(_localizer[message], Severity.Error);
            }

            Wait = false;
        }
    }

    private void Register()
    {
        _navigationManager.NavigateTo("/register");
    }

    bool PasswordVisibility;
    InputType PasswordInput = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    void TogglePasswordVisibility()
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
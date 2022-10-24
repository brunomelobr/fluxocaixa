using BrunoMelo.FluxoCaixa.Client.Services.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using NuvTools.Common.ResultWrapper;
using NuvTools.Security.Identity.Models.Form;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Authentication;

public partial class Register
{
    [Inject] UserService Service { get; set; }

    public EditContext EditContext { get; set; }
    private UserWithPasswordForm ObjetoEdicao { get; set; } = new();

    protected override void OnInitialized()
    {
        EditContext = new(ObjetoEdicao);
    }

    private async Task SubmitAsync()
    {
        if (!EditContext.Validate()) return;

        ObjetoEdicao.Status = true;
        ObjetoEdicao.EmailConfirmed = false;

        IResult response = await Service.SaveAsync(ObjetoEdicao);
        if (response.Succeeded)
        {
            _snackBar.Add(_localizer[response.Messages[0]], Severity.Success);
            _navigationManager.NavigateTo(Routes.Security.Login);
        }
        else
        {
            _snackBar.Add(response.Messages[0], Severity.Error);
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
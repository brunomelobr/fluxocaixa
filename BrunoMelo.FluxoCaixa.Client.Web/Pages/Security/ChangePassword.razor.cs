using MudBlazor;
using NuvTools.Resources;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security;

public partial class ChangePassword
{
    private readonly NuvTools.Security.Identity.Models.Form.ChangePasswordForm passwordModel = new();

    private async Task ChangePasswordAsync()
    {
        var response = await _accountManager.ChangePasswordAsync(passwordModel);
        if (response.Succeeded)
        {
            _snackBar.Add(_localizerMessages[nameof(Messages.OperationPerformedSuccessfully)], Severity.Success);
            passwordModel.Password = string.Empty;
            passwordModel.NewPassword = string.Empty;
            passwordModel.ConfirmNewPassword = string.Empty;
        }
        else
        {
            foreach (var error in response.Messages)
            {
                _snackBar.Add(_localizer[error], Severity.Error);
            }
        }
    }
}
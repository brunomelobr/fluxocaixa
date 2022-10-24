using BrunoMelo.FluxoCaixa.Client.Services.Security;
using BrunoMelo.FluxoCaixa.Models.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using NuvTools.Common.Validations.Annotations;
using NuvTools.Resources;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security;

public partial class ResetPassword
{
    [Inject] UserService Service { get; set; }

    public class ObjectView
    {
        [Display(Name = nameof(Fields.Email), ResourceType = typeof(Fields))]
        [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
        [MaxLength(100, ErrorMessageResourceName = nameof(Messages.XMustBeUpToYCharacters), ErrorMessageResourceType = typeof(Messages))]
        [EmailAddress(ErrorMessageResourceName = nameof(Messages.InvalidEmail), ErrorMessageResourceType = typeof(Messages))]
        public string Email { get; set; }

        [Display(Name = nameof(Fields.Password), ResourceType = typeof(Fields))]
        [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
        [MaxLength(40, ErrorMessageResourceName = nameof(Messages.XMustBeUpToYCharacters), ErrorMessageResourceType = typeof(Messages))]
        [MinLength(6, ErrorMessageResourceName = nameof(Messages.XShouldHaveAtLeastYCharacters), ErrorMessageResourceType = typeof(Messages))]
        [PasswordComplexityCapitalLetters(1)]
        [PasswordComplexityLowerCaseLetters(1)]
        [PasswordComplexityDigits(1)]
        public string Password { get; set; }

        [Display(Name = nameof(Fields.PasswordConfirm), ResourceType = typeof(Fields))]
        [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
        [Compare(nameof(Password), ErrorMessageResourceName = nameof(Messages.XShouldBeEqualY), ErrorMessageResourceType = typeof(Messages))]
        public string ConfirmPassword { get; set; }
    }

    [Parameter]
    public string Token { get; set; }

    public EditContext EditContext { get; set; }
    public ObjectView EditModel { get; set; } = new ObjectView();

    protected override void OnInitialized()
    {
        EditContext = new(EditModel);

        var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out var param))
        {
            Token = param.First();
        }
    }

    private async Task SubmitAsync()
    {
        if (!string.IsNullOrEmpty(Token))
        {
            if (!EditContext.Validate()) return;

            var request = new NuvTools.Security.Identity.Models.Form.ResetPasswordForm { Email = EditModel.Email, Password = EditModel.Password, Token = Token };
            var result = await Service.ResetPasswordAsync(request);
            if (result.Succeeded)
            {
                _snackBar.Add(_localizerMessages[result.Messages[0]], Severity.Success);
                _navigationManager.NavigateTo("/");
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(_localizerMessages[message], Severity.Error);
                }
            }
        }
        else
        {
            _snackBar.Add(_localizerMessages[nameof(Messages.TokenNotFound)], Severity.Error);
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
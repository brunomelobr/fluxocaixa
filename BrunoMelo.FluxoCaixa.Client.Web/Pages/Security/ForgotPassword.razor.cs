using BrunoMelo.FluxoCaixa.Client.Services.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using NuvTools.Resources;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security;

public partial class ForgotPassword
{
    [Inject] UserService Service { get; set; }

    private bool Wait { get; set; }

    public class ObjectView
    {
        [Display(Name = nameof(Fields.Email), ResourceType = typeof(Fields))]
        [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
        [MaxLength(50, ErrorMessageResourceName = nameof(Messages.XMustBeUpToYCharacters), ErrorMessageResourceType = typeof(Messages))]
        [EmailAddress(ErrorMessageResourceName = nameof(Messages.InvalidEmail), ErrorMessageResourceType = typeof(Messages))]
        public string Email { get; set; }
    }


    public EditContext EditContext { get; set; }
    public ObjectView EditModel { get; set; } = new ObjectView();

    protected override void OnInitialized()
    {
        EditContext = new(EditModel);
    }

    private async Task SubmitAsync()
    {
        if (!EditContext.Validate()) return;

        var request = new NuvTools.Security.Identity.Models.Form.ForgotPasswordForm() { Email = EditModel.Email };
        var result = await Service.ForgotPasswordAsync(request);
        if (result.Succeeded)
        {
            _snackBar.Add(_localizerMessages[result.Messages[0]], Severity.Success);
            _navigationManager.NavigateTo(Routes.Security.Login);
        }
        else
        {
            foreach (var message in result.Messages)
            {
                _snackBar.Add(_localizerMessages[message], Severity.Error);
            }
        }
    }

    private void GoBack()
    {
        _navigationManager.NavigateTo(Routes.Security.Login);
    }
}
using BrunoMelo.FluxoCaixa.Client.Services.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NuvTools.Security.Identity.Models.Form;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Pages.Security
{
    public partial class Users
    {
        [Inject] UserService Service { get; set; }

        public List<UserForm> ListModelView = new();

        private string searchString = "";

        protected override async Task OnInitializedAsync()
        {
            await GetUsersAsync();
        }

        private async Task GetUsersAsync()
        {
            ListModelView = await Service.GetAllAsync();
        }

        private bool Search(UserForm user)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (user.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private async Task InvokeModal()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<UserEditModal>("Modal", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetUsersAsync();
            }
        }

        private void ViewProfile(int userId)
        {
            _navigationManager.NavigateTo(Routes.Security.GetUserProfile(userId));
        }

        private void ManageRoles(int userId)
        {
            _navigationManager.NavigateTo(Routes.Security.GetUserRoles(userId));
        }
    }
}
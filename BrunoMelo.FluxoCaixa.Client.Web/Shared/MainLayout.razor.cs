using MudBlazor;
using System;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Shared
{
    public partial class MainLayout : IDisposable
    {
        private MudTheme currentTheme;

        protected override async Task OnInitializedAsync()
        {
            _interceptor.RegisterEvent();
            currentTheme = await _preferenceManager.GetCurrentThemeAsync();
        }

        public void Dispose()
        {
            _interceptor.DisposeEvent();
        }

    }
}
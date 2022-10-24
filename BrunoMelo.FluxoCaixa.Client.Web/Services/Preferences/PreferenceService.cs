using Blazored.LocalStorage;
using BrunoMelo.FluxoCaixa.Client.Web.Infrastructure.Settings;
using MudBlazor;
using System.Threading.Tasks;

namespace BrunoMelo.FluxoCaixa.Client.Web.Services.Preferences;

public class PreferenceService
{
    private readonly ILocalStorageService _localStorageService;

    public PreferenceService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<bool> ToggleDarkModeAsync()
    {
        var preference = await GetPreference();
        preference.IsDarkMode = !preference.IsDarkMode;
        await SetPreference(preference);
        return !preference.IsDarkMode;
    }

    public async Task ChangeLanguageAsync(string languageCode)
    {
        var preference = await GetPreference();
        preference.LanguageCode = languageCode;
        await SetPreference(preference);
    }

    public async Task<MudTheme> GetCurrentThemeAsync()
    {
        var preference = await GetPreference();
        if (preference.IsDarkMode) return TemaPadrao.DarkTheme;
        return TemaPadrao.DefaultTheme;
    }

    public async Task<Preference> GetPreference()
    {
        return await _localStorageService.GetItemAsync<Preference>("preference") ?? new Preference();
    }

    public async Task SetPreference(Preference preference)
    {
        await _localStorageService.SetItemAsync("preference", preference);
    }
}
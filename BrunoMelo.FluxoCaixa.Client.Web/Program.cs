using BrunoMelo.FluxoCaixa.Client.Web.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Threading.Tasks;
using BrunoMelo.FluxoCaixa.Client.Web.Services.Preferences;

namespace BrunoMelo.FluxoCaixa.Client.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        var host = builder.AddApplication().Build();

        var storageService = host.Services.GetRequiredService<PreferenceService>();

        if (storageService != null)
        {
            CultureInfo culture;
            var preference = await storageService.GetPreference();
            if (preference != null)
                culture = new CultureInfo(preference.LanguageCode);
            else
                culture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        await builder.Build().RunAsync();
    }

}
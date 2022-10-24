using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using BrunoMelo.FluxoCaixa.Data;
using Microsoft.Extensions.DependencyInjection;
using NuvTools.AspNetCore.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace BrunoMelo.FluxoCaixa.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder ConfigureApplication(
        this IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.ConfigureSwagger();
        }

        app.UseStaticFiles();

        var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(localizationOptions.Value);

        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints();

        app.DatabaseMigrate<Contexto>();

        app.Initialize();

        return app;
    }


    private static void ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "BrunoMelo.FluxoCaixa.API");
            options.RoutePrefix = "swagger";
            options.DisplayRequestDuration();
        });
    }

    private static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }

    private static IApplicationBuilder Initialize(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var seeder = serviceScope.ServiceProvider.GetService<ContextoSeeder>();

        seeder.Initialize();

        return app;
    }
}
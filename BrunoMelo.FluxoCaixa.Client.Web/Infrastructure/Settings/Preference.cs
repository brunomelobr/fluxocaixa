﻿namespace BrunoMelo.FluxoCaixa.Client.Web.Infrastructure.Settings;

public record Preference
{
    public bool IsDarkMode { get; set; }
    public bool IsRTL { get; set; }
    public bool IsDrawerOpen { get; set; }
    public string PrimaryColor { get; set; }
    public string LanguageCode { get; set; } = "pt-BR";
}
﻿using MudBlazor;

namespace BrunoMelo.FluxoCaixa.Client.Web.Infrastructure.Settings;

public class TemaPadrao
{
    private static readonly Typography DefaultTypography = new()
    {
        Default = new Default()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".875rem",
            FontWeight = 400,
            LineHeight = 1.43,
            LetterSpacing = ".01071em"
        },
        H1 = new H1()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = "6rem",
            FontWeight = 300,
            LineHeight = 1.167,
            LetterSpacing = "-.01562em"
        },
        H2 = new H2()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = "3.75rem",
            FontWeight = 300,
            LineHeight = 1.2,
            LetterSpacing = "-.00833em"
        },
        H3 = new H3()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = "3rem",
            FontWeight = 400,
            LineHeight = 1.167,
            LetterSpacing = "0"
        },
        H4 = new H4()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = "2.125rem",
            FontWeight = 400,
            LineHeight = 1.235,
            LetterSpacing = ".00735em"
        },
        H5 = new H5()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = "1.5rem",
            FontWeight = 400,
            LineHeight = 1.334,
            LetterSpacing = "0"
        },
        H6 = new H6()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = "1.25rem",
            FontWeight = 400,
            LineHeight = 1.6,
            LetterSpacing = ".0075em"
        },
        Button = new Button()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".875rem",
            FontWeight = 500,
            LineHeight = 1.75,
            LetterSpacing = ".02857em"
        },
        Body1 = new Body1()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = "1rem",
            FontWeight = 400,
            LineHeight = 1.5,
            LetterSpacing = ".00938em"
        },
        Body2 = new Body2()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".875rem",
            FontWeight = 400,
            LineHeight = 1.43,
            LetterSpacing = ".01071em"
        },
        Caption = new Caption()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".75rem",
            FontWeight = 400,
            LineHeight = 1.66,
            LetterSpacing = ".03333em"
        },
        Subtitle2 = new Subtitle2()
        {
            FontFamily = new[] { "Montserrat", "Helvetica", "Arial", "sans-serif" },
            FontSize = ".875rem",
            FontWeight = 500,
            LineHeight = 1.57,
            LetterSpacing = ".00714em"
        }
    };

    private static readonly LayoutProperties DefaultLayoutProperties = new()
    {
        DefaultBorderRadius = "3px"
    };

    public static MudTheme DefaultTheme = new()
    {
        Palette = new Palette()
        {
            Primary = "#153389",
            Background = "#F5F4F4",
            DrawerBackground = Colors.Shades.White,
            DrawerText = "rgba(0,0,0, 0.7)",
            Success = Colors.Green.Darken3,
            Secondary = "#264CBA",
            Info = Colors.Grey.Lighten4
        },
        Typography = DefaultTypography,
        LayoutProperties = DefaultLayoutProperties
    };

    public static MudTheme DarkTheme = new()
    {
        Palette = new Palette()
        {
            Primary = "#1E88E5",
            Success = "#007E33",
            Black = "#27272f",
            Background = "#32333d",
            BackgroundGrey = "#27272f",
            Surface = "#373740",
            DrawerBackground = "#27272f",
            DrawerText = "rgba(255,255,255, 0.50)",
            AppbarBackground = "#27272f",
            AppbarText = "rgba(255,255,255, 0.70)",
            TextPrimary = "rgba(255,255,255, 0.70)",
            TextSecondary = "rgba(255,255,255, 0.50)",
            ActionDefault = "#adadb1",
            ActionDisabled = "rgba(255,255,255, 0.26)",
            ActionDisabledBackground = "rgba(255,255,255, 0.12)",
            DrawerIcon = "rgba(255,255,255, 0.50)"
        },
        Typography = DefaultTypography,
        LayoutProperties = DefaultLayoutProperties
    };
}
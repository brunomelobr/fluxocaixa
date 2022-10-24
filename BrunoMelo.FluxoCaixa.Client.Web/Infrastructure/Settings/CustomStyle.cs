using System.Collections.Generic;

namespace BrunoMelo.FluxoCaixa.Client.Web.Infrastructure.Settings;

public static class CustomStyle
{
    public static string Build()
    {
        return string.Empty;
    }
    public static string PrimarySmoothColor()
    {
        return "#E2F5FF";
    }
    public static string SecondarySmoothColor()
    {
        return "#FDEECF";
    }
    public static string TertiarySmoothColor()
    {
        return "#FAFAFA";
    }

    public static string PrimaryStrongColor()
    {
        return "#657C87";
    }

    public static string SecondaryStrongColor()
    {
        return "#6A5B15";
    }
    public static string QuaternarySmoothColor()
    {
        return "#EFEFEF";
    }

    public static string BackgroundQuaternaryColor(this string cssStyle)
    {
        return $"{cssStyle} background-color:{QuaternarySmoothColor()};";
    }

    public static string BackgroundPrimaryColor(this string cssStyle)
    {
        return $"{cssStyle} background-color:{PrimarySmoothColor()};";
    }
    public static string BackgroundSecondaryColor(this string cssStyle)
    {
        return $"{cssStyle} background-color:{SecondarySmoothColor()};";
    }
    public static string BackgroundTertiaryColor(this string cssStyle)
    {
        return $"{cssStyle} background-color:{TertiarySmoothColor()};";
    }

    public static string Width(this string cssStyle, int size)
    {
        return $"{cssStyle} width:{size}px;";
    }

    public static string Height(this string cssStyle, int size)
    {
        return $"{cssStyle} height:{size}px;";
    }

    public static string WidthHeight(this string cssStyle, int size)
    {
        return cssStyle.Width(size).Height(size);
    }

    public static string PaperKpiWidth(this string cssStyle)
    {
        return cssStyle.Width(200);
    }
    public static string PaperKpiHeight(this string cssStyle)
    {
        return cssStyle.Height(140);
    }
    public static string PaperKpiPrimary(this string cssStyle)
    {
        return cssStyle.BackgroundPrimaryColor().PaperKpiWidth();
    }
    public static string PaperKpiSecondary(this string cssStyle)
    {
        return cssStyle.BackgroundSecondaryColor().PaperKpiWidth();
    }
    public static string PaperKpiTitle(this string cssStyle)
    {
        return $"{cssStyle} {cssStyle.WordBreak()} color:#201F1A;";
    }

    public static string PaperKpiPrimaryTitle(this string cssStyle)
    {
        return $"{cssStyle} {cssStyle.WordBreak()} color:{PrimaryStrongColor()};";
    }

    public static string PaperKpiSecundaryTitle(this string cssStyle)
    {
        return $"{cssStyle} {cssStyle.WordBreak()} color:{SecondaryStrongColor()};";
    }

    public static string WordBreak(this string cssStyle)
    {
        return $"{cssStyle} break-word;";
    }

    public static string TextAlignCenter(this string cssStyle)
    {
        return $"{cssStyle} text-align: center;";
    }
    public static string TableHeader(this string cssStyle)
    {
        return cssStyle.BackgroundQuaternaryColor().TextAlignCenter();
    }
    public static string LoadingAnimation(this string cssStyle)
    {
        return $"{cssStyle} width:70px;height:70px;display:flex;justify-content:center;align-items:center;top:45%;position:fixed";
    }

    ///criar lista de 24 cores
    public static List<string> ListColor()
    {
        return new List<string>()
            {
            "#24B2FF","#FF9800","#363636",
            "#6A5ACD","#00BFFF","#0000FF",
            "#008B8B","#008000","#808000",
            "#8B4513","#4B0082","#8B008B",
            "#DC143C","#8B0000","#FF00FF",
            "#FFA500","#4682B4","#FF0000",
            "#000000","#008080","#FFFF00",
            "#006400",

        };
    }
}
namespace BrunoMelo.FluxoCaixa.Client.Web.Infrastructure.Settings;

public static class MudBlazorCssClass
{
    public static string Build()
    {
        return string.Empty;
    }
    public static string Hide(this string cssClass)
    {
        return $"{cssClass} d-none";
    }
    public static string Margin(this string cssClass, short size)
    {
        return $"{cssClass} ma-{size}";
    }
    public static string MarginBottom(this string cssClass, short size)
    {
        return $"{cssClass} mb-{size}";
    }
    public static string MarginTop(this string cssClass, short size)
    {
        return $"{cssClass} mt-{size}";
    }
    public static string MarginLeft(this string cssClass, short size)
    {
        return $"{cssClass} ml-{size}";
    }
    public static string MarginRight(this string cssClass, short size)
    {
        return $"{cssClass} mr-{size}";
    }
    public static string MarginLeftRight(this string cssClass, short size)
    {
        return $"{cssClass} mx-{size}";
    }
    public static string MarginBottomTop(this string cssClass, short size)
    {
        return $"{cssClass} my-{size}";
    }
    public static string MarginBottomNegative(this string cssClass, short size)
    {
        return $"{cssClass} mb-n{size}";
    }
    public static string Padding(this string cssClass, short size)
    {
        return $"{cssClass} pa-{size}";
    }
    public static string PaddingLeft(this string cssClass, short size)
    {
        return $"{cssClass} pl-{size}";
    }
    public static string PaddingRight(this string cssClass, short size)
    {
        return $"{cssClass} pr-{size}";
    }
    public static string PaddingBottom(this string cssClass, short size)
    {
        return $"{cssClass} pb-{size}";
    }
    public static string PaddingBottomNegative(this string cssClass, short size)
    {
        return $"{cssClass} pb-n{size}";
    }
    public static string PaddingBottomTop(this string cssClass, short size)
    {
        return $"{cssClass} py-{size}";
    }
    public static string PaddingLeftRight(this string cssClass, short size)
    {
        return $"{cssClass} px-{size}";
    }
    public static string MarginTopNegative(this string cssClass, short size)
    {
        return $"{cssClass} mt-n{size}";
    }
    public static string Flex(this string cssClass)
    {
        return $"{cssClass} d-flex";
    }
    public static string FlexRow(this string cssClass)
    {
        return $"{cssClass} flex-row";
    }

    public static string FlexColumn(this string cssClass)
    {
        return $"{cssClass} flex-column";
    }

    public static string FlexInline(this string cssClass)
    {
        return $"{cssClass} d-inline-flex";
    }
    public static string FlexRowReverse(this string cssClass)
    {
        return $"{cssClass} flex-row-reverse";
    }
    public static string AlignCenter(this string cssClass)
    {
        return $"{cssClass} align-center";
    }

    public static string AlignTop(this string cssClass)
    {
        return $"{cssClass} align-top";
    }

    public static string AlignSelfTop(this string cssClass)
    {
        return $"{cssClass} align-self-top";
    }

    public static string AlignContentCenter(this string cssClass)
    {
        return $"{cssClass} align-content-center";
    }

    public static string AlignSelfCenter(this string cssClass)
    {
        return $"{cssClass} align-self-center";
    }

    public static string JustifyCenter(this string cssClass)
    {
        return $"{cssClass} justify-center";
    }

    public static string JustifyEnd(this string cssClass)
    {
        return $"{cssClass} justify-end";
    }

    public static string Wrap(this string cssClass)
    {
        return $"{cssClass} flex-wrap";
    }

    #region Custom Classes
    public static string TitleBarActions(this string cssClass)
    {
        return cssClass.FlexInline().AlignCenter().FlexRowReverse().MarginBottom(1);
    }
    public static string TitleBarTableActions(this string cssClass)
    {
        return cssClass.FlexInline().AlignCenter().FlexRowReverse().MarginBottom(2);
    }
    public static string ColumnActions(this string cssClass)
    {
        return cssClass.Flex().FlexRowReverse();
    }
    public static string ModalActions(this string cssClass)
    {
        return cssClass.MarginTop(2).MarginLeftRight(1).MarginBottom(1);
    }
    public static string ActionsRight(this string cssClass)
    {
        return cssClass.FlexInline().JustifyEnd();
    }
    public static string ModalTitleIcon(this string cssClass)
    {
        return cssClass.MarginRight(3).MarginBottomNegative(1);
    }
    public static string PaperKpi(this string cssClass)
    {
        return cssClass.Padding(3).AlignSelfCenter().Margin(2);
    }
    public static string PaperKpiValue(this string cssClass)
    {
        return cssClass.PaddingBottom(6);
    }
    public static string PaperTextList(this string cssClass)
    {
        return cssClass.Padding(3).MarginLeftRight(2).MarginBottomTop(0);
    }
    #endregion
}

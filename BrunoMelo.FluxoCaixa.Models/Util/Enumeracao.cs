using System.ComponentModel;

namespace BrunoMelo.FluxoCaixa.Models.Util;

public static partial class Enumeracao
{
    public enum TipoTransacao : short
    {
        [Description("Crédito")]
        Credito = 1,

        [Description("Débito")]
        Debito = 2
    }
    public enum Role
    {
        [Description("Administrador")]
        Administrador = 1,
        [Description("Usuário")]
        Usuario = 2
    }

    public enum Categoria
    {
        [Description("Alimentação")]
        Alimentação = 1,
        [Description("Lazer")]
        Lazer = 2,
        [Description("Casa")]
        Casa = 3,
        [Description("Transporte")]
        Transporte = 4
    }

    public enum TipoUsuarioSistema
    {
        [Description("Sistema")]
        Sistema = 1
    }
}
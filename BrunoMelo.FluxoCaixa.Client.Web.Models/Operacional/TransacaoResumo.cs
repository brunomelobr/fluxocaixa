using System;

namespace BrunoMelo.FluxoCaixa.Client.Web.Models.Operacional;

public class TransacaoResumo
{
    public long TransacaoId { get; set; }

    public short TipoTransacaoId { get; set; }

    public string NomeTipoTransacao { get; set; }

    public string Descricao { get; set; }

    public DateTime Data { get; set; }

    public decimal Valor { get; set; }

    public string NomeCategoria { get; set; }

    public string NomeConta { get; set; }
}

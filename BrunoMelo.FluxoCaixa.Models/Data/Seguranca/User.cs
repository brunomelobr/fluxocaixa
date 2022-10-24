using System.ComponentModel.DataAnnotations.Schema;
using NuvTools.Security.Identity.Models;

namespace BrunoMelo.FluxoCaixa.Models.Data.Seguranca;

[Table("User", Schema = "Seguranca")]
public class User : UserBase<int>
{
    public short? SistemaId { get; set; }
}
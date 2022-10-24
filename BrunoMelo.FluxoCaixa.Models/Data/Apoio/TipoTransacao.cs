using NuvTools.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrunoMelo.FluxoCaixa.Models.Data.Apoio;

[Table("TipoTransacao", Schema = "Apoio")]
public class TipoTransacao
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public short TipoTransacaoId { get; set; }

    [Required]
    [Display(Name = nameof(Fields.Name), ResourceType = typeof(Fields))]
    [MaxLength(30, ErrorMessageResourceName = nameof(Messages.XMustBeUpToYCharacters), ErrorMessageResourceType = typeof(Messages))]
    public string Nome { get; set; }
}

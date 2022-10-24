using NuvTools.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrunoMelo.FluxoCaixa.Models.Data.Manutencao;

[Table("Conta", Schema = "Manutencao")]
public class Conta
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContaId { get; set; }

    [Display(Name = nameof(Fields.Name), ResourceType = typeof(Fields))]
    [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
    [MaxLength(60, ErrorMessageResourceName = nameof(Messages.XMustBeUpToYCharacters), ErrorMessageResourceType = typeof(Messages))]
    public string Nome { get; set; }
}

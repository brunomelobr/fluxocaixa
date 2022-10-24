using NuvTools.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrunoMelo.FluxoCaixa.Models.Data.Manutencao;

[Table("Categoria", Schema = "Manutencao")]
public class Categoria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoriaId { get; set; }

    [Required]
    [Display(Name = nameof(Fields.Name), ResourceType = typeof(Fields))]
    [MaxLength(30, ErrorMessageResourceName = nameof(Messages.XMustBeUpToYCharacters), ErrorMessageResourceType = typeof(Messages))]
    public string Nome { get; set; }
}

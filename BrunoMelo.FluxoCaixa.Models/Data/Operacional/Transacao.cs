using BrunoMelo.FluxoCaixa.Models.Data.Apoio;
using BrunoMelo.FluxoCaixa.Models.Data.Manutencao;
using BrunoMelo.FluxoCaixa.Resources;
using NuvTools.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrunoMelo.FluxoCaixa.Models.Data.Operacional;

[Table("Transacao", Schema = "Operacional")]
public class Transacao
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long TransacaoId { get; set; }

    public short TipoTransacaoId { get; set; }

    [ForeignKey("TipoTransacaoId")]
    public virtual TipoTransacao TipoTransacao { get; set; }


    [Display(Name = nameof(Fields.Description), ResourceType = typeof(Fields))]
    [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
    [MaxLength(60, ErrorMessageResourceName = nameof(Messages.XMustBeUpToYCharacters), ErrorMessageResourceType = typeof(Messages))]
    public string Descricao { get; set; }

    [Display(Name = nameof(Fields.Value), ResourceType = typeof(Fields))]
    [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Valor { get; set; }

    [Display(Name = nameof(Fields.Date), ResourceType = typeof(Fields))]
    [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
    public DateTime Data { get; set; }

    [Display(Name = nameof(CampoEspecifico.Categoria), ResourceType = typeof(CampoEspecifico))]
    [Required(ErrorMessageResourceName = nameof(Messages.XRequired), ErrorMessageResourceType = typeof(Messages))]
    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    public virtual Categoria Categoria { get; set; }

    public int ContaId { get; set; }

    [ForeignKey("ContaId")]
    public virtual Conta Conta { get; set; }
}
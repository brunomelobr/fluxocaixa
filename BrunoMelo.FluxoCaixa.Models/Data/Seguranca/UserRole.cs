using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrunoMelo.FluxoCaixa.Models.Data.Seguranca;

[Table("UserRole", Schema = "Seguranca")]
public class UserRole : IdentityUserRole<int>
{
}
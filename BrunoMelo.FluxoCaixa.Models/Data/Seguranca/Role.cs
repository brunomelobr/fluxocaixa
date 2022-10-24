using NuvTools.Security.Identity.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrunoMelo.FluxoCaixa.Models.Data.Seguranca;

[Table("Role", Schema = "Seguranca")]
public class Role : RoleBase<int>
{
    public Role() : base()
    {
    }

    public Role(string roleName) : base(roleName)
    {

    }
}
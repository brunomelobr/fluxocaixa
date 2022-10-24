using BrunoMelo.FluxoCaixa.Models.Data.Apoio;
using BrunoMelo.FluxoCaixa.Models.Data.Seguranca;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NuvTools.Common.Enums;
using BrunoMelo.FluxoCaixa.Models.Util;
using BrunoMelo.FluxoCaixa.Models.Data.Manutencao;

namespace BrunoMelo.FluxoCaixa.Data;

public partial class Contexto
{
    #region Carga - Apoio

    private static void Seed(EntityTypeBuilder<TipoTransacao> builder)
    {
        if (builder is null) return;

        builder.HasData(
                    new TipoTransacao
                    {
                        TipoTransacaoId = (short)Enumeracao.TipoTransacao.Credito,
                        Nome = Enumeracao.TipoTransacao.Credito.GetDescription()
                    },
                    new TipoTransacao
                    {
                        TipoTransacaoId = (short)Enumeracao.TipoTransacao.Debito,
                        Nome = Enumeracao.TipoTransacao.Debito.GetDescription()
                    });
    }

    #endregion

    #region Carga - Manutenção
    private static void Seed(EntityTypeBuilder<Conta> builder)
    {
        if (builder is null) return;

        builder.HasData(
            new Conta { ContaId = 1, Nome = "Conta Corrente" },
            new Conta { ContaId = 2, Nome = "Carteira" }
            );
    }

    private static void Seed(EntityTypeBuilder<Categoria> builder)
    {
        if (builder is null) return;

        builder.HasData(
            new Categoria { CategoriaId = 1, Nome = "Alimentação" },
            new Categoria { CategoriaId = 2, Nome = "Lazer" },
            new Categoria { CategoriaId = 3, Nome = "Transporte" },
            new Categoria { CategoriaId = 4, Nome = "Saúde" },
            new Categoria { CategoriaId = 5, Nome = "Salário" },
            new Categoria { CategoriaId = 6, Nome = "Dividendos" }
            );
    }

    #endregion

    #region Carga - Segurança

    private static void Seed(EntityTypeBuilder<Role> builder)
    {
        if (builder is null) return;

        builder.HasData(
                    new Role
                    {
                        Id = (short)Enumeracao.Role.Administrador,
                        Name = Enumeracao.Role.Administrador.GetDescription(),
                        NormalizedName = Enumeracao.Role.Administrador.GetDescription()
                    },
                    new Role
                    {
                        Id = (short)Enumeracao.Role.Usuario,
                        Name = Enumeracao.Role.Usuario.GetDescription(),
                        NormalizedName = Enumeracao.Role.Usuario.GetDescription()
                    }
                    );
    }

    #endregion
}

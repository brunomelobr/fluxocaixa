using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BrunoMelo.FluxoCaixa.Models.Data.Operacional;
using BrunoMelo.FluxoCaixa.Models.Data.Apoio;
using BrunoMelo.FluxoCaixa.Models.Data.Manutencao;
using BrunoMelo.FluxoCaixa.Models.Data.Seguranca;

namespace BrunoMelo.FluxoCaixa.Data;

public partial class Contexto : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) {
    }

    #region DbSet - Apoio
    public virtual DbSet<Categoria> Categoria { get; set; }
    public virtual DbSet<TipoTransacao> TipoTransacao { get; set; }
    #endregion

    #region DbSet - Manutencao
    public virtual DbSet<Conta> Conta { get; set; }
    #endregion

    #region DbSet - Operacional
    public virtual DbSet<Transacao> Transacao { get; set; }
    #endregion

    #region DbSet - Segurança
    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<Role> Role { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null) throw new ArgumentNullException(nameof(modelBuilder));

        base.OnModelCreating(modelBuilder);

        ConfigurarNomeTabela(modelBuilder);
        ConfigurarIndex(modelBuilder);
        ConfigurarCascade(modelBuilder);

        RealizarCarga(modelBuilder);
    }

    private static void ConfigurarNomeTabela(ModelBuilder modelBuilder)
    {
        #region Segurança

        modelBuilder.Entity<User>()
                               .ToTable("User", "Seguranca");

        modelBuilder.Entity<Role>()
                                .ToTable("Role", "Seguranca");

        modelBuilder.Entity<UserRole>()
                                .ToTable("UserRole", "Seguranca");

        modelBuilder.Entity<IdentityUserClaim<int>>()
                                .ToTable("IdentityUserClaim", "Seguranca");

        modelBuilder.Entity<IdentityUserLogin<int>>()
                                .ToTable("IdentityUserLogin", "Seguranca");

        modelBuilder.Entity<IdentityRoleClaim<int>>()
                                .ToTable("IdentityRoleClaim", "Seguranca");

        modelBuilder.Entity<IdentityUserToken<int>>()
                                .ToTable("IdentityUserToken", "Seguranca");

        #endregion
    }

    private static void ConfigurarIndex(ModelBuilder modelBuilder)
    {
        #region Apoio
        modelBuilder.Entity<Categoria>().HasIndex(e => e.Nome).IsUnique().HasDatabaseName("IX_Categoria");
        modelBuilder.Entity<TipoTransacao>().HasIndex(e => e.Nome).IsUnique().HasDatabaseName("IX_TipoTransacao");
        #endregion

        #region Manutenção
        modelBuilder.Entity<Conta>().HasIndex(e => e.Nome).IsUnique().HasDatabaseName("IX_Conta");
        #endregion
    }

    private static void ConfigurarCascade(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoTransacao>().HasMany<Transacao>().WithOne(a => a.TipoTransacao).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Categoria>().HasMany<Transacao>().WithOne(a => a.Categoria).OnDelete(DeleteBehavior.Restrict);
    }

    private static void RealizarCarga(ModelBuilder modelBuilder)
    {
        Seed(modelBuilder.Entity<Categoria>());
        Seed(modelBuilder.Entity<TipoTransacao>());
        Seed(modelBuilder.Entity<Conta>());

        #region Carga - Segurança

        Seed(modelBuilder.Entity<Role>());

        #endregion
    }
}
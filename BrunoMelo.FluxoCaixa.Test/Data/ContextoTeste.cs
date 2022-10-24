using BrunoMelo.FluxoCaixa.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using NuvTools.Data.EntityFrameworkCore.Design;
using System;

namespace BrunoMelo.FluxoCaixa.Test.Data;

public class ContextoTeste
{
    public class DesignTimeDbContextFactory : DesignTimeDbContextFactoryBase<Contexto>
    {
        protected override Contexto CreateNewInstance(DbContextOptionsBuilder<Contexto> optionsBuilder)
        {
            if (optionsBuilder is null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            InitializeConfiguration(new JsonConfigurationSource { Path = "appsettings.json" });

            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Principal"));

            return new Contexto(optionsBuilder.Options);
        }
    }
}

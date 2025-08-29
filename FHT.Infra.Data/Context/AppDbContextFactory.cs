using FHT.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var basePath = Directory.GetCurrentDirectory();

        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false)
            .AddUserSecrets<AppDbContextFactory>(optional: true) 
            .AddEnvironmentVariables()
            .Build();

        var provider = config["Database:Provider"] ?? "SqlServer";
        var conn = config.GetConnectionString(provider);
        var migAsm = config["Database:MigrationsAssembly"]; 

        if (string.IsNullOrWhiteSpace(conn))
            throw new InvalidOperationException($"ConnectionStrings:{provider} não configurada no appsettings do projeto de startup.");

        var options = new DbContextOptionsBuilder<AppDbContext>();

        options.UseSqlServer(conn, sql =>
        {
            if (!string.IsNullOrWhiteSpace(migAsm))
                sql.MigrationsAssembly(migAsm);

            sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            sql.CommandTimeout(60);

            sql.MigrationsHistoryTable("HISTORICO_MIGRACAO", "dbo");
        });

#if DEBUG
        options.EnableSensitiveDataLogging();
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
#endif

        return new AppDbContext(options.Options);
    }
}

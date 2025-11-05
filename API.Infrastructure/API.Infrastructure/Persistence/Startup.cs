
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using ILogger = Serilog.ILogger;
using DAL;
//using DAL.Helpers;
using System.Data;
using API.Infrastructure.Common.Contract;
using DAL.Core.Application.Persistence;

namespace API.Infrastructure.Persistence
{
    internal static class Startup
    {
        private static readonly ILogger _settingsger = Log.ForContext(typeof(Startup));

        internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<DatabaseSettings>()
                .BindConfiguration(nameof(DatabaseSettings))
                .PostConfigure(databaseSettings =>
                {
                    _settingsger.Information("Current DB Provider: {dbProvider}", databaseSettings.DBProvider);
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();
            return services
           .AddDbContext<ApplicationDbContext>((p, m) =>
           {
             var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
             //  string converteddata =
            System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(databaseSettings.Cypher));
               //  string encryptedconstring = EncryptDecrypt.Decrypt(databaseSettings.ConnectionString, converteddata);
               string encryptedconstring = databaseSettings.ConnectionString;

               m.UseSqlServer(encryptedconstring,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure()
                       .MigrationsAssembly("LumoDevice.API");
                    });
               
           }).AddRepositories();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Add Repositories
            services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));

            foreach (var aggregateRootType in
                typeof(IAggregateRoot).Assembly.GetExportedTypes()
                    .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
                    .ToList())
            {
                // Add ReadRepositories.
                services.AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRootType), sp =>
                    sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)));

                // Decorate the repositories with EventAddingRepositoryDecorators and expose them as IRepositoryWithEvents.
                //services.AddScoped(typeof(IRepositoryWithEvents<>).MakeGenericType(aggregateRootType), sp =>
                //    Activator.CreateInstance(
                //        typeof(EventAddingRepositoryDecorator<>).MakeGenericType(aggregateRootType),
                //        sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)))
                //    ?? throw new InvalidOperationException($"Couldn't create EventAddingRepositoryDecorator for aggregateRootType {aggregateRootType.Name}"));
            }

            return services;
        }
        //internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        //{

        //            return builder.UseSqlServer(connectionString, e =>
        //                 e.MigrationsAssembly("Migrators.MSSQL"));
        //        //case DbProviderKeys.Oracle:
        //        //    return builder.UseSqlServer(connectionString, e =>
        //        //         e.MigrationsAssembly("Migrators.MSSQL"));



        //}
    }
}

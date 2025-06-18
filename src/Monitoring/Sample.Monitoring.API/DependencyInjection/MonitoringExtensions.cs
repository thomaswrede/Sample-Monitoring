using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Sample.Monitoring.Infrastructure.Health;

namespace Sample.Monitoring.API.DependencyInjection
{
  /// <summary>
  /// Stellt Erweiterungsmethoden zur Verfügung, um die Monitoring-Datenbank und zugehörige Dienste zu registrieren und zu initialisieren.
  /// </summary>
  public static class MonitoringExtensions
  {
    #region " öffentliche Methoden                        "

    #region " --> AddMonitoringStorage                    "
    /// <summary>
    /// Registriert den <see cref="HealthCheckDbContext"/> für die Überwachungsspeicherung und stellt sicher, dass der Datenbankordner existiert.
    /// </summary>
    /// <param name="services">Die <see cref="IServiceCollection"/>, zu der die Dienste hinzugefügt werden.</param>
    /// <returns>Die aktualisierte <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddMonitoringStorage(this IServiceCollection services)
    {
      IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

      // Sicherstellen, dass der Ordner für die SQLite-Datenbank existiert, da sonst die Migration fehlschlägt
      SqliteConnectionStringBuilder builder = new(configuration.GetConnectionString("MonitoringDb"));
      FileInfo fileInfo = new(builder.DataSource);
      if (!fileInfo.Directory.Exists) { fileInfo.Directory.Create(); }

      // DBContext für die Health Checks registrieren
      services
        .AddDbContext<HealthCheckDbContext>(options => options
        .UseSqlite(configuration.GetConnectionString("MonitoringDb"),
            x => x.MigrationsHistoryTable("__MigrationHistory", "Monitoring"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

      return services;
    }
    #endregion

    #region " --> UseMonitoringStorage                    "
    /// <summary>
    /// Führt ausstehende Migrationen für den <see cref="HealthCheckDbContext"/> aus, um die Datenbankstruktur zu erstellen oder zu aktualisieren.
    /// </summary>
    /// <param name="builder">Die <see cref="IApplicationBuilder"/>-Instanz.</param>
    /// <returns>Die aktualisierte <see cref="IApplicationBuilder"/>-Instanz.</returns>
    public static IApplicationBuilder UseMonitoringStorage(this IApplicationBuilder builder)
    {
      // HealthCheckDbContext migrieren, damit die Datenbankstruktur angelegt und aktuell ist
      using (IServiceScope scope = builder.ApplicationServices.CreateScope())
      {
        HealthCheckDbContext dbContext = scope.ServiceProvider.GetRequiredService<HealthCheckDbContext>();
        if (dbContext?.Database.GetPendingMigrations().Any() ?? false)
        {
          dbContext.Database.Migrate();
        }
      }

      return builder;
    }
    #endregion

    #endregion
  }
}

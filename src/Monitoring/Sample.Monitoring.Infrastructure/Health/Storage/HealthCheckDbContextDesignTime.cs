using System;

using Sample.Monitoring.Infrastructure.Health;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sample.Monitoring.Infrastructure.Storage
{
  /// <summary>
  /// Stellt eine Design-Time-Factory für <see cref="HealthCheckDbContext"/> bereit.
  /// Wird von Entity Framework Core Tools verwendet, um Migrationen und andere Design-Time-Vorgänge auszuführen.
  /// </summary>
  public class HealthCheckDbContextDesignTime : IDesignTimeDbContextFactory<HealthCheckDbContext>
  {
    /// <summary>
    /// Erstellt eine neue Instanz von <see cref="HealthCheckDbContext"/> für Design-Time-Vorgänge.
    /// </summary>
    /// <param name="args">Argumente, die von der Umgebung übergeben werden.</param>
    /// <returns>Eine neue <see cref="HealthCheckDbContext"/>-Instanz mit konfigurierten Optionen.</returns>
    public HealthCheckDbContext CreateDbContext(String[] args)
    {
      DbContextOptionsBuilder<HealthCheckDbContext> optionsBuilder = new();
      _ = optionsBuilder.UseSqlite("Data Source=.\\App_Data\\monitoring.db");

      return new HealthCheckDbContext(optionsBuilder.Options);
    }
  }
}

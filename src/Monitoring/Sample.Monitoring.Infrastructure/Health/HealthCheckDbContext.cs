using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Sample.Monitoring.Infrastructure.Health.Configuration;
using Sample.Monitoring.Model.Health;

namespace Sample.Monitoring.Infrastructure.Health
{
  /// <summary>
  /// Stellt den Entity Framework Core DbContext für HealthCheck-bezogene Entitäten bereit.
  /// Verwaltet HealthChecks, HealthCheckEntries und HealthCheckHistoryEntries.
  /// </summary>
  public class HealthCheckDbContext(DbContextOptions<HealthCheckDbContext> options) : DbContext(options), IAsyncDisposable, IDisposable
  {
    #region " Variablen/ Properties                       "

    #region " --> IsDisposed                              "
    /// <summary>
    /// Gibt an, ob der Kontext bereits entsorgt wurde.
    /// </summary>
    private Boolean _IsDisposed;
    #endregion

    #region " --> ConnectionString                        "
    /// <summary>
    /// Die Verbindungszeichenfolge für die Datenbank.
    /// </summary>
    protected readonly ReadOnlyMemory<Char> _ConnectionString;
    #endregion

    #region " --> HealthChecks                            "
    /// <summary>
    /// DbSet für HealthCheck-Entitäten.
    /// </summary>
    public DbSet<HealthCheck> HealthChecks { get; set; }
    #endregion

    #region " --> HealthCheckEntries                      "
    /// <summary>
    /// DbSet für HealthCheckEntry-Entitäten.
    /// </summary>
    public DbSet<HealthCheckEntry> HealthCheckEntries { get; set; }
    #endregion

    #region " --> HealthCheckHistoryEntries               "
    /// <summary>
    /// DbSet für HealthCheckHistoryEntry-Entitäten.
    /// </summary>
    public DbSet<HealthCheckHistoryEntry> HealthCheckHistoryEntries { get; set; }
    #endregion

    #endregion

    #region " Konstruktor/ Destruktor                     "

    #region " --> Dispose                                 "
    /// <summary>
    /// Gibt die von der <see cref="HealthCheckDbContext"/> Klasse verwendeten Ressourcen frei.
    /// </summary>
    public override void Dispose()
    {
      if (this._IsDisposed) { return; }
      this.Dispose(true);
      base.Dispose();
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gibt die von der <see cref="HealthCheckDbContext"/> Klasse verwendeten Ressourcen asynchron frei.
    /// </summary>
    /// <returns>Ein Task, der den asynchronen Vorgang darstellt.</returns>
    public async ValueTask DisposeAsnc()
    {
      await this.DisposeAsyncCore().ConfigureAwait(false);
      this.Dispose(false);
#pragma warning disable CA1816 // Dispose-Methoden müssen SuppressFinalize aufrufen
      GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose-Methoden müssen SuppressFinalize aufrufen
    }

    /// <summary>
    /// Schließt die Datenbankverbindung asynchron.
    /// </summary>
    /// <returns>Ein Task, der den asynchronen Vorgang darstellt.</returns>
    private async ValueTask DisposeAsyncCore() => await this.Database.CloseConnectionAsync();

    /// <summary>
    /// Gibt die von der <see cref="HealthCheckDbContext"/> Klasse verwendeten Ressourcen frei.
    /// </summary>
    /// <param name="disposing">Gibt an, ob verwaltete Ressourcen freigegeben werden sollen.</param>
    private void Dispose(Boolean disposing)
    {
      if (this._IsDisposed) { return; }
      if (disposing)
      {
        this.Database.CloseConnection();
      }
      this._IsDisposed = true;
    }
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> RegisterHealthCheckAsync                "
    /// <summary>
    /// Registriert einen neuen HealthCheck, falls dieser noch nicht existiert.
    /// </summary>
    /// <param name="healthCheck">Das zu registrierende <see cref="HealthCheck"/>-Objekt.</param>
    /// <returns>Das registrierte oder bereits vorhandene <see cref="HealthCheck"/>-Objekt.</returns>
    public async Task<HealthCheck> RegisterHealthCheckAsync(HealthCheck healthCheck)
    {
      HealthCheck check = await this.HealthChecks.FirstOrDefaultAsync(e => e.Name == healthCheck.Name);
      if (check == default)
      {
        check = new()
        {
          Id = Guid.NewGuid(),
          Name = healthCheck.Name,
          Description = healthCheck.Description,
          Tags = healthCheck.Tags
        };
        _ = await this.AddAsync(check);
        _ = await this.SaveChangesAsync();
      }
      return check;
    }
    #endregion

    #region " --> AddHealthCheckAsync                     "
    /// <summary>
    /// Fügt einen neuen HealthCheckEntry hinzu oder aktualisiert einen bestehenden Eintrag.
    /// Legt bei Statusänderung einen neuen History-Eintrag an.
    /// </summary>
    /// <param name="healthCheckEntry">Der hinzuzufügende oder zu aktualisierende <see cref="HealthCheckEntry"/>.</param>
    /// <returns>Ein Task, der den asynchronen Vorgang darstellt.</returns>
    public async Task AddHealthCheckAsync(HealthCheckEntry healthCheckEntry)
    {
      HealthCheckEntry entry = await this.HealthCheckEntries.FirstOrDefaultAsync(e => e.ApplicationId == healthCheckEntry.ApplicationId && e.ServerName == healthCheckEntry.ServerName && e.Name == healthCheckEntry.Name);
      HealthCheckHistoryEntry historyEntry = default;
      if (entry == default)
      {
        HealthCheck check = await this.HealthChecks.FirstOrDefaultAsync(e => e.Name == healthCheckEntry.Name);

        entry = new()
        {
          Id = Guid.NewGuid(),
          ApplicationId = healthCheckEntry.ApplicationId,
          HealthCheckId = check.Id,
          ServerName = healthCheckEntry.ServerName,
          Name = healthCheckEntry.Name,
          Description = healthCheckEntry.Description,
          Status = healthCheckEntry.Status,
          LastDuration = healthCheckEntry.LastDuration,
          LastExecution = DateTime.Now
        };
        historyEntry = new()
        {
          Id = Guid.NewGuid(),
          EntryId = entry.Id,
          Status = entry.Status,
          StatusFrom = DateTime.Now,
          LastDuration = entry.LastDuration,
          LastExecution = entry.LastExecution
        };
        _ = await this.AddAsync(entry);
        _ = await this.AddAsync(historyEntry);
        _ = await this.SaveChangesAsync();
      }
      else
      {
        historyEntry = this.HealthCheckHistoryEntries.Where(e => e.EntryId == entry.Id).OrderByDescending(e => e.LastExecution).FirstOrDefault();
        if (historyEntry.Status != healthCheckEntry.Status)
        {
          historyEntry = new()
          {
            Id = Guid.NewGuid(),
            EntryId = entry.Id,
            Status = healthCheckEntry.Status,
            StatusFrom = DateTime.Now,
            LastExecution = DateTime.Now,
            LastDuration = healthCheckEntry.LastDuration
          };
          _ = await this.AddAsync(historyEntry);
        }

        _ = this.Update(entry);
        entry.Description = healthCheckEntry.Description;
        entry.Status = healthCheckEntry.Status;
        entry.LastExecution = DateTime.Now;
        entry.LastDuration = healthCheckEntry.LastDuration;
        _ = await this.SaveChangesAsync(true);
      }
    }
    #endregion

    #region " --> GetHealthChecksAsync                    "
    /// <summary>
    /// Ruft die letzten HealthCheckHistoryEntries für die angegebenen HealthChecks ab.
    /// </summary>
    /// <param name="serviceOptions">Die HealthCheckServiceOptions mit den Registrierungen.</param>
    /// <param name="predicate">Ein Prädikat zur Filterung der HealthCheckRegistrations.</param>
    /// <returns>Eine Auflistung der letzten <see cref="HealthCheckHistoryEntry"/>-Einträge pro HealthCheck.</returns>
    public async Task<IEnumerable<HealthCheckHistoryEntry>> GetHealthChecksAsync(HealthCheckServiceOptions serviceOptions, Func<HealthCheckRegistration, Boolean> predicate)
    {
      IEnumerable<String> healthNames = [.. serviceOptions.Registrations.Where(predicate).Select(e => e.Name).Distinct()];
      List<HealthCheckHistoryEntry> result = [];

      await healthNames.ForAllAsync(async name => result.Add(await this
        .HealthCheckHistoryEntries
        .Include("HealthCheckEntry")
        .Where(e => name == e.HealthCheckEntry.Name)
        .OrderByDescending(e => e.LastExecution)
        .FirstOrDefaultAsync()));
      return result;
    }
    #endregion

    #endregion

    #region " interne Methoden                            "

    #region " --> OnModelCreating                         "
    /// <summary>
    /// Konfiguriert die Entitätszuordnungen für die HealthCheck-bezogenen Entitäten.
    /// </summary>
    /// <param name="modelBuilder">Der ModelBuilder für die Konfiguration.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder) => _ = modelBuilder
      .ApplyConfiguration(new HealthCheckMap())
      .ApplyConfiguration(new HealthCheckEntryMap())
      .ApplyConfiguration(new HealthCheckHistoryEntryMap());
    #endregion

    #endregion

  }
}

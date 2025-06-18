using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace Sample.Monitoring.Health.Application
{
  /// <summary>
  /// HealthCheck zur Überwachung des Anwendungsstatus.
  /// Meldet <see cref="HealthStatus.Healthy"/> solange die Anwendung nicht gestoppt wird.
  /// </summary>
  public class HealthCheckApplicationStatus : IHealthCheck, IDisposable
  {
    #region " Variablen/ Properties                       "

    #region " --> IsDisposed                              "
    /// <summary>
    /// Gibt an, ob das Objekt bereits entsorgt wurde.
    /// </summary>
    private Boolean _IsDisposed;
    #endregion

    #region " --> ApplicationLifetime                     "
    /// <summary>
    /// Instanz zur Überwachung des Lebenszyklus der Anwendung.
    /// </summary>
    private readonly IHostApplicationLifetime _ApplicationLifetime;
    #endregion

    #region " --> CancellationToken                       "
    /// <summary>
    /// Registrierung für das ApplicationStopping-Ereignis.
    /// </summary>
    private readonly CancellationTokenRegistration? _CancellationToken;
    #endregion

    #endregion

    #region " Konstruktor/ Destruktor                     "

    #region " --> New                                     "
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="HealthCheckApplicationStatus"/> Klasse.
    /// Registriert sich für das ApplicationStopping-Ereignis.
    /// </summary>
    /// <param name="applicationLifetime">Der Lebenszyklus der Anwendung.</param>
    public HealthCheckApplicationStatus(IHostApplicationLifetime applicationLifetime)
    {
      this._ApplicationLifetime = applicationLifetime;
      this._CancellationToken = this._ApplicationLifetime?.ApplicationStopping.Register(this.Dispose);
    }
    #endregion

    #region " --> Dispose                                 "
    /// <summary>
    /// Gibt die von der Instanz verwendeten Ressourcen frei.
    /// </summary>
    public void Dispose()
    {
      if (this._IsDisposed) { return; }
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gibt die von der Instanz verwendeten Ressourcen frei.
    /// </summary>
    /// <param name="disposing">Gibt an, ob verwaltete Ressourcen freigegeben werden sollen.</param>
    private void Dispose(Boolean disposing)
    {
      if (this._IsDisposed) { return; }
      if (disposing)
      {
        this._CancellationToken?.Dispose();
      }
      this._IsDisposed = true;
    }
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> CheckHealthAsync                        "
    /// <summary>
    /// Überprüft den aktuellen Gesundheitsstatus der Anwendung.
    /// Gibt <see cref="HealthStatus.Healthy"/> zurück, solange die Anwendung läuft.
    /// </summary>
    /// <param name="context">Der HealthCheck-Kontext.</param>
    /// <param name="cancellationToken">Token zum Abbrechen des Vorgangs.</param>
    /// <returns>Ein Task mit dem HealthCheckResult.</returns>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) => Task.FromResult(this._CancellationToken != default ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy());
    #endregion

    #endregion

  }
}

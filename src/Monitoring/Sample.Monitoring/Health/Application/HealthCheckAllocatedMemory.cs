using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health.Application
{
  /// <summary>
  /// HealthCheck zur Überwachung des aktuell allokierten Speichers im Prozess.
  /// Gibt einen Fehlerstatus zurück, wenn der zugewiesene Speicher einen definierten Grenzwert überschreitet.
  /// </summary>
  public class HealthCheckAllocatedMemory(Int64 maximumMemory) : IHealthCheck
  {
    #region " Variablen/ Properties                       "

    #region " --> MaximumMemory                           "
    /// <summary>
    /// Maximale erlaubte Menge an allokiertem Speicher (in MByte), bevor der HealthCheck fehlschlägt.
    /// </summary>
    private readonly Int64 _MaximumMemory = maximumMemory;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> CheckHealthAsync                        "
    /// <summary>
    /// Führt die Überprüfung des aktuell allokierten Speichers asynchron aus.
    /// Gibt <see cref="HealthStatus.Unhealthy"/> zurück, wenn der Speicherverbrauch den Grenzwert überschreitet.
    /// </summary>
    /// <param name="context">Der Kontext für den HealthCheck.</param>
    /// <param name="cancellationToken">Token zum Abbrechen des Vorgangs.</param>
    /// <returns>
    /// Ein <see cref="HealthCheckResult"/>, der den Status des HealthChecks beschreibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">Wird ausgelöst, wenn <paramref name="context"/> null ist.</exception>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) => context == null
        ? throw new ArgumentNullException(nameof(context))
        : await Task.Run(() =>
        {
          try
          {
            Int64 totalMemory = GC.GetTotalMemory(false) / 1024 / 1024;
            return totalMemory >= this._MaximumMemory
              ? HealthCheckResult.Unhealthy(description: $"allocated memory is about {totalMemory} mbytes, expected maximum is {this._MaximumMemory} mbytes")
              : HealthCheckResult.Healthy();
          }
          catch (Exception ex)
          {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
          }
        });
    #endregion

    #endregion
  }
}

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Implementiert einen HealthCheck für SignalR-Verbindungen.
  /// Baut eine Verbindung zu einem SignalR-Hub auf und prüft, ob diese erfolgreich hergestellt werden kann.
  /// </summary>
  public class HealthCheckSignalR(Func<HubConnection> hubConnectionBuilder) : IHealthCheck
  {
    #region " Variablen/ Properties                       "

    #region " --> HubConnectionBuilder                    "
    /// <summary>
    /// Funktion zum Erstellen einer neuen <see cref="HubConnection"/>.
    /// </summary>
    private readonly Func<HubConnection> _HubConnectionBuilder = hubConnectionBuilder;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> CheckHealthAsync                        "
    /// <summary>
    /// Führt den HealthCheck aus, indem eine SignalR-Verbindung aufgebaut und getestet wird.
    /// </summary>
    /// <param name="context">Der HealthCheck-Kontext.</param>
    /// <param name="cancellationToken">Token zum Abbrechen des Vorgangs.</param>
    /// <returns>
    /// Ein <see cref="HealthCheckResult"/>, der den Status der SignalR-Verbindung angibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">Wird ausgelöst, wenn <paramref name="context"/> null ist.</exception>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(context);

      HubConnection connection = default;

      try
      {
        connection = this._HubConnectionBuilder();
        await connection.StartAsync(cancellationToken).ConfigureAwait(false);

        return HealthCheckResult.Healthy();
      }
      catch (Exception ex)
      {
        return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
      }
      finally
      {
        if (connection != null)
        {
          await connection.DisposeAsync().ConfigureAwait(false);
        }
      }
    }
    #endregion

    #endregion

  }
}

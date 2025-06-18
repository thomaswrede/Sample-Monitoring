using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Führt einen DNS-Health-Check für eine Liste von Hostnamen durch.
  /// </summary>
  /// <param name="hostNames">Die zu überprüfenden Hostnamen.</param>
  public class HealthCheckDns(IEnumerable<String> hostNames) : IHealthCheck
  {
    #region " Variablen/ Properties                       "

    #region " --> HostNames                               "
    /// <summary>
    /// Die Liste der zu überprüfenden Hostnamen.
    /// </summary>
    private readonly IEnumerable<String> _HostNames = hostNames;

    #endregion
    #endregion

    #region " öffentliche Methoden                        "

    #region " --> CheckHealthAsync                        "
    /// <summary>
    /// Überprüft asynchron, ob alle angegebenen Hostnamen per DNS aufgelöst werden können.
    /// </summary>
    /// <param name="context">Der HealthCheck-Kontext.</param>
    /// <param name="cancellationToken">Token zum Abbrechen des Vorgangs.</param>
    /// <returns>
    /// Ein <see cref="HealthCheckResult"/>, der den Status des Health-Checks angibt.
    /// </returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(context);

      try
      {
        foreach (String hostName in this._HostNames)
        {
          IPAddress[] ipAddresses = await Dns.GetHostAddressesAsync(hostName, cancellationToken);
          if (ipAddresses.Length == 0)
          {
            return new HealthCheckResult(context.Registration.FailureStatus, description: $"host {hostName} was not resolved from dns server");
          }
        }
        return HealthCheckResult.Healthy();
      }
      catch (Exception ex)
      {
        return new HealthCheckResult(HealthStatus.Unhealthy, exception: ex);
      }
    }
    #endregion

    #endregion

  }
}

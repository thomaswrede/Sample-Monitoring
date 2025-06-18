using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Implementiert einen REST-basierten HealthCheck, der konfigurierbare Hosts und Services abfragt.
  /// Prüft die Erreichbarkeit und Authentifizierung von REST-Endpunkten und gibt den Gesundheitsstatus zurück.
  /// </summary>
  /// <remarks>
  /// Die zu prüfenden Hosts, Services und optionalen Zugangsdaten werden über <see cref="HealthCheckRestOptions"/> bereitgestellt.
  /// </remarks>
  public class HealthCheckRest(HealthCheckRestOptions options) : IHealthCheck
  {
    #region " Variablen/ Properties                       "

    #region " --> Options                                 "
    /// <summary>
    /// Konfigurationseinstellungen für die zu prüfenden REST-Endpunkte.
    /// </summary>
    private readonly HealthCheckRestOptions _Options = options;

    #endregion
    #endregion

    #region " öffentliche Methoden                        "

    #region " --> CheckHealthAsync                        "
    /// <summary>
    /// Führt die Überprüfung der konfigurierten REST-Endpunkte durch und gibt das Ergebnis als <see cref="HealthCheckResult"/> zurück.
    /// </summary>
    /// <param name="context">Der Kontext des HealthChecks, enthält Registrierungsinformationen.</param>
    /// <param name="cancellationToken">Token zur Abbruchsteuerung des asynchronen Vorgangs.</param>
    /// <returns>
    /// Ein <see cref="HealthCheckResult"/> mit dem Status <c>Healthy</c>, <c>Degraded</c> oder <c>Unhealthy</c>,
    /// abhängig von den Ergebnissen der REST-Abfragen.
    /// </returns>
    /// <exception cref="ArgumentNullException">Wird ausgelöst, wenn <paramref name="context"/> null ist.</exception>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(context);

      List<String> errorMessages = [];
      List<String> warnMessages = [];
      try
      {
        foreach ((String Host, String Service, NetworkCredential Credentials) in this._Options.Hosts.Values)
        {
          HttpClientHandler clientHandler = new() { ServerCertificateCustomValidationCallback = (_, _, _, _) => true };
          using (HttpClient client = new(clientHandler))
          {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(String.Concat(Host, Service), cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
              if (response.StatusCode == HttpStatusCode.Unauthorized)
              {
                warnMessages.Add($"rest api {Host}{Service} returns status code {response.StatusCode}");
              }
              else
              {
                errorMessages.Add($"rest api {Host}{Service} returns status code {response.StatusCode}");
              }
            }
            //TODO: Credentials
          }
        }

        if (errorMessages.Count != 0)
        {
          return new HealthCheckResult(context.Registration.FailureStatus, description: String.Join(';', errorMessages));
        }
        else if (warnMessages.Count != 0)
        {
          return new HealthCheckResult(HealthStatus.Degraded, description: String.Join(';', warnMessages));
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

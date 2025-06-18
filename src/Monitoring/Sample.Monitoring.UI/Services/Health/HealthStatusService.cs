using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Sample.Monitoring.Model.Health;

using Microsoft.Extensions.Configuration;

namespace Sample.Monitoring.UI.Services.Health
{
  /// <summary>
  /// Service zum Abrufen des aktuellen Health-Status einer Anwendung über die API.
  /// </summary>
  /// <remarks>
  /// Initialisiert eine neue Instanz des <see cref="HealthStatusService"/>.
  /// </remarks>
  /// <param name="httpClientFactory">Die Factory zur Erstellung von <see cref="HttpClient"/>-Instanzen.</param>
  /// <param name="configuration">Die Anwendungskonfiguration zur Ermittlung der ApplicationId.</param>
  public sealed class HealthStatusService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
  {
    #region " Variablen/ Properties                       "

    #region " --> ApplicationId                           "
    /// <summary>
    /// Die eindeutige Id der Anwendung, deren Health-Status abgefragt werden soll.
    /// </summary>
    private readonly Guid _ApplicationId = configuration.GetValue<Guid>("Health:ApplicationId");
    #endregion

    #region " --> HttpClientFactory                       "
    /// <summary>
    /// Die Factory zur Erstellung von <see cref="HttpClient"/>-Instanzen für API-Aufrufe.
    /// </summary>
    private readonly IHttpClientFactory _HttpClientFactory = httpClientFactory;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> GetHealthStatusAsync                    "
    /// <summary>
    /// Ruft asynchron den aktuellen Health-Status der Anwendung von der API ab.
    /// </summary>
    /// <returns>
    /// Eine <see cref="Task"/> mit einer Auflistung von <see cref="HealthCheckStateView"/>-Objekten,
    /// die den aktuellen Zustand der Health-Checks repräsentieren.
    /// </returns>
    public async Task<IEnumerable<HealthCheckStateView>> GetHealthStatusAsync()
    {
      HttpClient httpClient = this._HttpClientFactory.CreateClient("Sample.Monitoring.API");
      return await httpClient.GetFromJsonAsync<IEnumerable<HealthCheckStateView>>($"api/HealthMonitor/GetApplicationStatus?applicationId={this._ApplicationId}");
    }
    #endregion

    #region " --> GetHealthCheckAsync                      "
    /// <summary>
    /// Ruft asynchron den Status eines bestimmten Health-Checks der Anwendung von der API ab.
    /// </summary>
    /// <param name="checkName">Der Name des Health-Checks, dessen Status abgefragt werden soll.</param>
    /// <returns>
    /// Eine <see cref="Task"/> mit dem <see cref="HealthCheckStateView"/>-Objekt,
    /// das den aktuellen Zustand des angegebenen Health-Checks repräsentiert.
    /// </returns>
    public async Task<HealthCheckStateView> GetHealthCheckAsync(String checkName)
    {
      HttpClient httpClient = this._HttpClientFactory.CreateClient("Sample.Monitoring.API");
      return await httpClient.GetFromJsonAsync<HealthCheckStateView>($"api/HealthMonitor/GetHealthCheck?applicationId={this._ApplicationId}&checkName={checkName}");
    }
    #endregion

    #endregion
  }
}

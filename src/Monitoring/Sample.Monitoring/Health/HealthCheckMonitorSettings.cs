using System;
using System.Collections.Generic;

namespace Sample.Monitoring.Health
{
  /// <summary>
  /// Stellt Konfigurationseinstellungen für die Überwachung von HealthChecks bereit.
  /// Enthält Informationen zur Storage API, Servernamen, Anwendungs-ID, Prüfintervall und die zu überwachenden HealthChecks.
  /// </summary>
  public class HealthCheckMonitorSettings
  {
    #region " Variablen/ Properties                       "

    #region " --> StorageApi                              "
    /// <summary>
    /// Die URL oder der Endpunkt der Storage API, an die HealthCheck-Ergebnisse gesendet oder von der sie abgerufen werden.
    /// </summary>
    public String StorageApi { get; set; }
    #endregion

    #region " --> ServerName                              "
    /// <summary>
    /// Der Name des Servers, auf dem die HealthChecks ausgeführt werden.
    /// </summary>
    public String ServerName { get; set; }
    #endregion

    #region " --> ApplicationId                           "
    /// <summary>
    /// Die eindeutige Kennung (GUID) der überwachten Anwendung.
    /// </summary>
    public Guid ApplicationId { get; set; }
    #endregion

    #region " --> CheckInterval                           "
    /// <summary>
    /// Prüfintervall in Sekunden.
    /// Standard: 10s
    /// </summary>
    public Int32 CheckInterval { get; set; } = 10;
    #endregion

    #region " --> HealthChecks                            "
    /// <summary>
    /// Liste der konfigurierten HealthChecks, die überwacht werden sollen.
    /// </summary>
    public IEnumerable<HealthCheckSettings> HealthChecks => this._HealthChecks;
    private readonly List<HealthCheckSettings> _HealthChecks = [];
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> AddHealthCheckOptions                   "
    /// <summary>
    /// Fügt eine HealthCheck-Konfiguration zu den überwachten HealthChecks hinzu.
    /// </summary>
    /// <param name="settings">Die hinzuzufügenden HealthCheck-Einstellungen.</param>
    /// <returns>Die aktuelle Instanz von <see cref="HealthCheckMonitorSettings"/> zur Verkettung.</returns>
    public HealthCheckMonitorSettings AddHealthCheckOptions(HealthCheckSettings settings)
    {
      this._HealthChecks.Add(settings);
      return this;
    }
    #endregion

    #endregion

  }
}

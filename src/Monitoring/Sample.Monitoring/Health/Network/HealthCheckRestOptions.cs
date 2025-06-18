using System;
using System.Collections.Generic;
using System.Net;

using Microsoft.Extensions.Configuration;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Stellt Konfigurationseinstellungen für REST-basierte HealthChecks bereit.
  /// Erweitert <see cref="HealthCheckSettings"/> um Host-spezifische Einstellungen.
  /// </summary>
  public class HealthCheckRestOptions : HealthCheckSettings
  {
    #region " Konstanten                                  "

    #region " --> CONFIG_SECTION_HOSTS                    "
    /// <summary>
    /// Konfigurationsabschnitt für Hosts.
    /// </summary>
    private const String CONFIG_SECTION_HOSTS = "Hosts";
    #endregion

    #region " --> CONFIG_VALUE_HOST                       "
    /// <summary>
    /// Konfigurationsschlüssel für den Hostnamen.
    /// </summary>
    private const String CONFIG_VALUE_HOST = "Host";
    #endregion

    #region " --> CONFIG_VALUE_SERVICE                    "
    /// <summary>
    /// Konfigurationsschlüssel für den Servicenamen.
    /// </summary>
    private const String CONFIG_VALUE_SERVICE = "Service";
    #endregion

    #endregion

    #region " Variablen/ Properties                       "

    #region " --> Hosts                                   "
    /// <summary>
    /// Enthält die konfigurierten Hosts mit zugehörigem Service und optionalen Zugangsdaten.
    /// </summary>
    internal Dictionary<String, (String Host, String Service, NetworkCredential Credentials)> Hosts { get; } = [];
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Init                                    "
    /// <summary>
    /// Initialisiert die <see cref="HealthCheckRestOptions"/>-Instanz mit Werten aus der angegebenen Konfigurationssektion.
    /// </summary>
    /// <param name="section">Die Konfigurationssektion mit den HealthCheck-Einstellungen.</param>
    /// <returns>Die initialisierte Instanz von <see cref="HealthCheckSettings"/>.</returns>
    public override HealthCheckSettings Init(IConfigurationSection section)
    {
      base.Bind(section);
      _ = section
        .GetSection(CONFIG_SECTION_HOSTS)?
        .GetChildren()
        .ForAll(c => this.AddHost(c.GetValue<String>(CONFIG_VALUE_HOST), c.GetValue<String>(CONFIG_VALUE_SERVICE)));
      return this;
    }
    #endregion

    #region " --> AddHost                                 "
    /// <summary>
    /// Fügt einen Host mit zugehörigem Service und optionalen Zugangsdaten zur Konfiguration hinzu.
    /// </summary>
    /// <param name="host">Der Hostname.</param>
    /// <param name="service">Der Servicename.</param>
    /// <param name="credentials">Optionale Zugangsdaten für den Host.</param>
    /// <returns>Die aktuelle Instanz von <see cref="HealthCheckRestOptions"/>.</returns>
    public HealthCheckRestOptions AddHost(String host, String service, NetworkCredential credentials = null)
    {
      this.Hosts.Add(host, (host, service, credentials));
      return this;
    }
    #endregion

    #endregion

  }
}

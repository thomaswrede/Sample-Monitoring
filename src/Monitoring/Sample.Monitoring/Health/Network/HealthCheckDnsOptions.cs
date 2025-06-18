using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Stellt Konfigurationseinstellungen für DNS-HealthChecks bereit.
  /// Erweitert <see cref="HealthCheckSettings"/> um die Möglichkeit, eine Liste von Hostnamen zu konfigurieren.
  /// </summary>
  public class HealthCheckDnsOptions : HealthCheckSettings
  {
    #region " Konstanten                                  "

    #region " --> CONFIG_SECTION_HOSTS                    "
    /// <summary>
    /// Konfigurationsschlüssel für den Abschnitt mit den zu prüfenden Hostnamen.
    /// </summary>
    private const String CONFIG_SECTION_HOSTS = "Hosts";
    #endregion

    #endregion

    #region " Variablen/ Properties                       "

    #region " --> HostNames                               "
    /// <summary>
    /// Liste der Hostnamen, die im Rahmen des DNS-HealthChecks überprüft werden sollen.
    /// </summary>
    public IEnumerable<String> HostNames => this._HostNames;
    /// <summary>
    /// Interne Liste zur Speicherung der Hostnamen.
    /// </summary>
    private readonly List<String> _HostNames = [];
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Init                                    "
    /// <summary>
    /// Initialisiert die <see cref="HealthCheckDnsOptions"/>-Instanz mit Werten aus der angegebenen Konfigurationssektion.
    /// Liest insbesondere die Hostnamen aus dem Abschnitt <c>Hosts</c> ein.
    /// </summary>
    /// <param name="section">Die Konfigurationssektion mit den DNS-HealthCheck-Einstellungen.</param>
    /// <returns>Die initialisierte Instanz von <see cref="HealthCheckDnsOptions"/>.</returns>
    public override HealthCheckSettings Init(IConfigurationSection section)
    {
      base.Bind(section);
      section
        .GetSection(CONFIG_SECTION_HOSTS)?
        .GetChildren()
        .ForAll(c => this._HostNames.Add(c.Value));
      return this;
    }
    #endregion

    #endregion

  }
}

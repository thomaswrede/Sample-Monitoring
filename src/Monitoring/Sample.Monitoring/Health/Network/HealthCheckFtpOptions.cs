using System;
using System.Collections.Generic;
using System.Net;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Stellt Optionen für FTP-Health-Checks bereit, einschließlich der Verwaltung von Hosts und deren Zugangsdaten.
  /// </summary>
  public class HealthCheckFtpOptions
  {
    #region " Variablen/ Properties                       "

    #region " --> Hosts                                   "
    /// <summary>
    /// Enthält die konfigurierten FTP-Hosts und deren zugehörige Zugangsdaten.
    /// Der Schlüssel ist der Hostname, der Wert ist ein Tupel aus Hostname und <see cref="NetworkCredential"/>.
    /// </summary>
    internal Dictionary<String, (String Host, NetworkCredential Credentials)> Hosts { get; } = [];
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> AddHost                                 "
    /// <summary>
    /// Fügt einen neuen FTP-Host mit optionalen Zugangsdaten zur Überwachungsliste hinzu.
    /// </summary>
    /// <param name="host">Der Hostname oder die Adresse des FTP-Servers.</param>
    /// <param name="credentials">Die Zugangsdaten für den FTP-Server (optional).</param>
    /// <returns>Die aktuelle <see cref="HealthCheckFtpOptions"/>-Instanz zur Verkettung.</returns>
    public HealthCheckFtpOptions AddHost(String host, NetworkCredential credentials = null)
    {
      this.Hosts.Add(host, (host, credentials));
      return this;
    }
    #endregion

    #endregion

  }
}

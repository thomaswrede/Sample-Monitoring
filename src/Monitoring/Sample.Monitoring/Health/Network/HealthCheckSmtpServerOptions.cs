using System;
using System.Net;

using Microsoft.Extensions.Configuration;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Stellt Konfigurationseinstellungen für einen SMTP-Server-HealthCheck bereit.
  /// </summary>
  public class HealthCheckSmtpServerOptions : HealthCheckSettings
  {
    #region " Variablen/ Properties                       "

    #region " --> SmtpSettings                            "
    /// <summary>
    /// Ruft die SMTP-spezifischen Einstellungen für den HealthCheck ab.
    /// </summary>
    public SmtpSettings SmtpSettings => this._SmtpSettings;
    private SmtpSettings _SmtpSettings;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Init                                    "
    /// <summary>
    /// Initialisiert die <see cref="HealthCheckSmtpServerOptions"/>-Instanz mit Werten aus der angegebenen Konfigurationssektion.
    /// </summary>
    /// <param name="section">Die Konfigurationssektion mit den SMTP-HealthCheck-Einstellungen.</param>
    /// <returns>Die initialisierte Instanz von <see cref="HealthCheckSmtpServerOptions"/>.</returns>
    public override HealthCheckSettings Init(IConfigurationSection section)
    {
      base.Bind(section);
      this._SmtpSettings = new(section.GetValue<String>("Hostname"),
        port: section.GetValue<Int32?>("Port"),
        enableSsl: section.GetValue<Boolean?>("EnableSsl"));
      return this;
    }
    #endregion

    #endregion

  }
}

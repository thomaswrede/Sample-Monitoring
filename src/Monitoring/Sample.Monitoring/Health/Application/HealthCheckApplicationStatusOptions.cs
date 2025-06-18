using Microsoft.Extensions.Configuration;

namespace Sample.Monitoring.Health.Application
{
  /// <summary>
  /// Stellt Konfigurationseinstellungen für den Application Status HealthCheck bereit.
  /// Erbt von <see cref="HealthCheckSettings"/> und ermöglicht die Initialisierung aus einer Konfigurationssektion.
  /// </summary>
  public class HealthCheckApplicationStatusOptions : HealthCheckSettings
  {
    #region " öffentliche Methoden                        "

    #region " --> Init                                    "
    /// <summary>
    /// Initialisiert die <see cref="HealthCheckApplicationStatusOptions"/>-Instanz mit Werten aus der angegebenen Konfigurationssektion.
    /// </summary>
    /// <param name="section">Die Konfigurationssektion mit den HealthCheck-Einstellungen.</param>
    /// <returns>Die initialisierte Instanz von <see cref="HealthCheckSettings"/>.</returns>
    public override HealthCheckSettings Init(IConfigurationSection section)
    {
      base.Bind(section);
      return this;
    }
    #endregion

    #endregion

  }
}

using System;

using Microsoft.Extensions.Configuration;

namespace Sample.Monitoring.Health.Application
{
  /// <summary>
  /// Konfigurationseinstellungen für den HealthCheck, der den allokierten Speicher überwacht.
  /// </summary>
  public class HealthCheckAllocatedMemoryOptions : HealthCheckSettings
  {
    #region " Konstanten                                  "

    #region " --> CONFIG_VALUE_MAXIMUM_MEMORY             "
    /// <summary>
    /// Konfigurationsschlüssel für den maximal erlaubten Speicher (in Byte).
    /// </summary>
    private const String CONFIG_VALUE_MAXIMUM_MEMORY = "MaximumMemory";
    #endregion

    #endregion

    #region " Variablen/ Properties                       "

    #region " --> MaximumMemory                           "
    /// <summary>
    /// Gibt den maximal erlaubten allokierten Speicher in Byte an.
    /// </summary>
    public Int64 MaximumMemory => this._MaximumMemory;
    private Int64 _MaximumMemory;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Init                                    "
    /// <summary>
    /// Initialisiert die <see cref="HealthCheckAllocatedMemoryOptions"/>-Instanz mit Werten aus der angegebenen Konfigurationssektion.
    /// </summary>
    /// <param name="section">Die Konfigurationssektion mit den HealthCheck-Einstellungen.</param>
    /// <returns>Die initialisierte Instanz von <see cref="HealthCheckSettings"/>.</returns>
    public override HealthCheckSettings Init(IConfigurationSection section)
    {
      base.Bind(section);
      this._MaximumMemory = section.GetValue<Int64?>(CONFIG_VALUE_MAXIMUM_MEMORY) ?? 32 * 1024; //32 GByte per default
      return this;
    }
    #endregion

    #endregion

  }
}

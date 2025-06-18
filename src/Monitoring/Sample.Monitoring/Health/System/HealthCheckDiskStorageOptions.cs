using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace Sample.Monitoring.Health.System
{
  /// <summary>
  /// Konfigurationseinstellungen für den HealthCheck zur Überwachung des freien Speicherplatzes auf Datenträgern.
  /// </summary>
  public class HealthCheckDiskStorageOptions : HealthCheckSettings
  {
    #region " Konstanten                                  "

    #region " --> CONFIG_SECTION_DRIVES                   "
    /// <summary>
    /// Konfigurationsabschnitt für die zu überwachenden Laufwerke.
    /// </summary>
    private const String CONFIG_SECTION_DRIVES = "Drives";
    #endregion

    #region " --> CONFIG_VALUE_DRIVE_NAME                 "
    /// <summary>
    /// Konfigurationsschlüssel für den Namen des Laufwerks.
    /// </summary>
    private const String CONFIG_VALUE_DRIVE_NAME = "DriveName";
    #endregion

    #region " --> CONFIG_VALUE_MINIMUM_FREE_SPACE         "
    /// <summary>
    /// Konfigurationsschlüssel für den minimalen freien Speicherplatz (in Byte).
    /// </summary>
    private const String CONFIG_VALUE_MINIMUM_FREE_SPACE = "MinimumFreeSpace";
    #endregion

    #endregion

    #region " Variablen/ Properties                       "

    #region " --> Drives                                  "
    /// <summary>
    /// Liste der zu überwachenden Laufwerke mit deren minimal erforderlichem freien Speicherplatz (in Byte).
    /// </summary>
    public List<(String DriveName, Int64 MinimumFreeSpace)> Drives { get; set; } = [];
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Init                                    "
    /// <summary>
    /// Initialisiert die <see cref="HealthCheckDiskStorageOptions"/>-Instanz mit Werten aus der angegebenen Konfigurationssektion.
    /// </summary>
    /// <param name="section">Die Konfigurationssektion mit den Einstellungen für die Laufwerksüberwachung.</param>
    /// <returns>Die initialisierte Instanz von <see cref="HealthCheckDiskStorageOptions"/>.</returns>
    public override HealthCheckSettings Init(IConfigurationSection section)
    {
      base.Bind(section);
      section
        .GetSection(CONFIG_SECTION_DRIVES)?
        .GetChildren()
        .Select(e => (DriveName: e.GetValue<String>(CONFIG_VALUE_DRIVE_NAME), MinimumFreeSpace: e.GetValue<Int64>(CONFIG_VALUE_MINIMUM_FREE_SPACE)))
        .ForAll(this.Drives.Add);
      return this;
    }
    #endregion

    #endregion

  }
}

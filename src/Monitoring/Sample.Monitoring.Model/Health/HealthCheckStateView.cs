using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Model.Health
{
  /// <summary>
  /// Stellt eine Ansicht des aktuellen Zustands von Health-Checks dar, gruppiert nach Hierarchieebenen und Check-Namen.
  /// </summary>
  public sealed class HealthCheckStateView
  {
    #region " Variablen/ Properties                       "

    #region " --> FirstLevel                              "
    /// <summary>
    /// Die erste Hierarchieebene, z. B. der Name der Anwendung oder Kategorie.
    /// </summary>
    [JsonPropertyName(nameof(FirstLevel))]
    public String FirstLevel { get; set; }
    #endregion

    #region " --> SecondLevel                             "
    /// <summary>
    /// Die zweite Hierarchieebene, z. B. der Name des Servers oder Subsystems.
    /// </summary>
    [JsonPropertyName(nameof(SecondLevel))]
    public String SecondLevel { get; set; }
    #endregion

    #region " --> CheckName                               "
    /// <summary>
    /// Der Name des Health-Checks.
    /// </summary>
    [JsonPropertyName(nameof(CheckName))]
    public String CheckName { get; set; }
    #endregion

    #region " --> Checks                                  "
    /// <summary>
    /// Liste der Health-Check-Einträge, die zu dieser Ansicht gehören.
    /// </summary>
    [JsonPropertyName(nameof(Checks))]
    public List<HealthCheckEntry> Checks { get; set; }
    #endregion

    #region " --> Status                                  "
    /// <summary>
    /// Aggregierter Status aller zugehörigen Health-Check-Einträge.
    /// Gibt <see cref="HealthStatus.Healthy"/> zurück, wenn alle Checks gesund sind,
    /// <see cref="HealthStatus.Degraded"/>, wenn mindestens einer degradiert ist,
    /// andernfalls <see cref="HealthStatus.Unhealthy"/>.
    /// </summary>
    //[JsonPropertyName(nameof(Status))]
    public HealthStatus Status => (this.Checks?.All(e => e.Status == HealthStatus.Healthy) ?? false) ? HealthStatus.Healthy : (this.Checks?.Any(e => e.Status == HealthStatus.Degraded) ?? false) ? HealthStatus.Degraded : HealthStatus.Unhealthy;
    #endregion

    #endregion

  }
}

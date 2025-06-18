using System;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Model.Health
{
  /// <summary>
  /// Repräsentiert einen Eintrag in der Historie eines Health-Checks.
  /// Enthält Informationen über Status, Zeitpunkte und Dauer einer Health-Check-Ausführung.
  /// </summary>
  public sealed class HealthCheckHistoryEntry : IUnique
  {
    #region " Variablen/ Properties                       "

    #region " --> Id                                      "
    /// <summary>
    /// Eindeutige Id des History-Eintrags.
    /// </summary>
    [JsonPropertyName(nameof(Id))]
    public Guid Id { get; set; }
    #endregion

    #region " --> EntryId                                 "
    /// <summary>
    /// Id des zugehörigen <see cref="HealthCheckEntry"/>.
    /// </summary>
    [JsonPropertyName(nameof(EntryId))]
    public Guid EntryId { get; set; }
    #endregion

    #region " --> Status                                  "
    /// <summary>
    /// Status des Health-Checks zum Zeitpunkt dieses Eintrags.
    /// </summary>
    [JsonPropertyName(nameof(Status))]
    public HealthStatus Status { get; set; }
    #endregion

    #region " --> StatusFrom                              "
    /// <summary>
    /// Zeitpunkt, ab dem der Status gültig ist.
    /// </summary>
    [JsonPropertyName(nameof(StatusFrom))]
    public DateTime StatusFrom { get; set; }
    #endregion

    #region " --> LastExecution                           "
    /// <summary>
    /// Zeitpunkt der letzten Ausführung des Health-Checks.
    /// </summary>
    [JsonPropertyName(nameof(LastExecution))]
    public DateTime LastExecution { get; set; }
    #endregion

    #region " --> LastDuration                            "
    /// <summary>
    /// Dauer der letzten Ausführung des Health-Checks.
    /// </summary>
    [JsonPropertyName(nameof(LastDuration))]
    public TimeSpan LastDuration { get; set; }
    #endregion

    #region " --> HealthCheckEntry                        "
    /// <summary>
    /// Referenz auf den zugehörigen <see cref="HealthCheckEntry"/>.
    /// Wird nicht serialisiert.
    /// </summary>
    [JsonIgnore]
    public HealthCheckEntry HealthCheckEntry { get; set; }
    #endregion

    #endregion
  }
}

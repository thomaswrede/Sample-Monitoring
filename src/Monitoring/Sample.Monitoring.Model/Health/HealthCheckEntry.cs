using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Model.Health
{
  /// <summary>
  /// Repräsentiert einen Eintrag für einen Health-Check einer Anwendung auf einem bestimmten Server.
  /// Enthält Informationen zum Status, Ausführungszeitpunkt, Dauer, zugehörigem HealthCheck und Historie.
  /// </summary>
  public sealed class HealthCheckEntry : IUnique
  {
    #region " Variablen/ Properties                       "

    #region " --> Id                                      "
    /// <summary>
    /// Eindeutige Id des HealthCheckEntry.
    /// </summary>
    [JsonPropertyName(nameof(Id))]
    public Guid Id { get; set; }
    #endregion

    #region " --> ApplicationId                           "
    /// <summary>
    /// Id der Anwendung, zu der dieser HealthCheckEntry gehört.
    /// </summary>
    [JsonPropertyName(nameof(ApplicationId))]
    public Guid ApplicationId { get; set; }
    #endregion

    #region " --> ServerName                              "
    /// <summary>
    /// Name des Servers, auf dem der Health-Check ausgeführt wird.
    /// </summary>
    [JsonPropertyName(nameof(ServerName))]
    public String ServerName
    {
      get => this._ServerName.ToString();
      set => this._ServerName = value?.AsMemory() ?? default;
    }
    /// <summary>
    /// Interner Zwischenspeicher für den Servernamen.
    /// </summary>
    private ReadOnlyMemory<Char> _ServerName;
    #endregion

    #region " --> Name                                    "
    /// <summary>
    /// Name des Health-Check-Eintrags.
    /// </summary>
    [JsonPropertyName(nameof(Name))]
    public String Name
    {
      get => this._Name.ToString();
      set => this._Name = value?.AsMemory() ?? default;
    }
    /// <summary>
    /// Interner Zwischenspeicher für den Namen.
    /// </summary>
    private ReadOnlyMemory<Char> _Name;
    #endregion

    #region " --> Description                             "
    /// <summary>
    /// Beschreibung des Health-Check-Eintrags.
    /// </summary>
    [JsonPropertyName(nameof(Description))]
    public String Description
    {
      get => this._Description.ToString();
      set => this._Description = value?.AsMemory() ?? default;
    }
    /// <summary>
    /// Interner Zwischenspeicher für die Beschreibung.
    /// </summary>
    private ReadOnlyMemory<Char> _Description;
    #endregion

    #region " --> Status                                  "
    /// <summary>
    /// Aktueller Status des Health-Checks.
    /// </summary>
    [JsonPropertyName(nameof(HealthStatus))]
    public HealthStatus Status { get; set; }
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

    #region " --> HealthCheck                             "
    /// <summary>
    /// Zugehöriger HealthCheck.
    /// </summary>
    [JsonPropertyName(nameof(HealthCheck))]
    public HealthCheck HealthCheck { get; set; }
    /// <summary>
    /// Id des zugehörigen HealthCheck (wird nicht serialisiert).
    /// </summary>
    [JsonIgnore]
    public Guid? HealthCheckId { get; set; }
    #endregion

    #region " --> HistoryEntries                          "
    /// <summary>
    /// Historie der Statusänderungen und Ausführungen dieses Health-Check-Eintrags.
    /// </summary>
    [JsonPropertyName(nameof(HistoryEntries))]
    public ICollection<HealthCheckHistoryEntry> HistoryEntries { get; set; }
    #endregion

    #endregion

  }
}

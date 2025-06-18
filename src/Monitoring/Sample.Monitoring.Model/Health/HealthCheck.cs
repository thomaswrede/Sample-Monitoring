using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sample.Monitoring.Model.Health
{
  /// <summary>
  /// Repräsentiert einen Health-Check mit eindeutiger Id, Name, Beschreibung und Tags.
  /// </summary>
  public sealed class HealthCheck : IUnique
  {
    #region " Variablen/ Properties                       "

    #region " --> Id                                      "
    /// <summary>
    /// Ruft die eindeutige Id des Health-Checks ab oder legt sie fest.
    /// </summary>
    [JsonPropertyName(nameof(Id))]
    public Guid Id { get; set; }
    #endregion

    #region " --> Name                                    "
    /// <summary>
    /// Ruft den Namen des Health-Checks ab oder legt ihn fest.
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
    /// Ruft die Beschreibung des Health-Checks ab oder legt sie fest.
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

    #region " --> Tags                                    "
    /// <summary>
    /// Ruft die Liste der zugeordneten Tags ab oder legt sie fest.
    /// </summary>
    [JsonPropertyName(nameof(Tags))]
    public List<String> Tags { get; set; }
    #endregion

    #endregion

  }
}

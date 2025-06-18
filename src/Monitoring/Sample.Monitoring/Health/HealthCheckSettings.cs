using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace Sample.Monitoring.Health
{
  /// <summary>
  /// Abstrakte Basisklasse für HealthCheck-Konfigurationseinstellungen.
  /// Stellt Eigenschaften und Methoden zur Verfügung, um HealthCheck-Settings aus einer Konfigurationssektion zu binden.
  /// </summary>
  public abstract class HealthCheckSettings
  {
    #region " Konstanten                                  "

    #region " --> CONFIG_VALUE_NAME                       "
    /// <summary>
    /// Konfigurationsschlüssel für den Namen des HealthChecks.
    /// </summary>
    private const String CONFIG_VALUE_NAME = "Name";
    #endregion

    #region " --> CONFIG_VALUE_DESCRIPTION                "
    /// <summary>
    /// Konfigurationsschlüssel für die Beschreibung des HealthChecks.
    /// </summary>
    private const String CONFIG_VALUE_DESCRIPTION = "Description";
    #endregion

    #region " --> CONFIG_VALUE_TYPE                       "
    /// <summary>
    /// Konfigurationsschlüssel für den Typ des HealthChecks.
    /// </summary>
    private const String CONFIG_VALUE_TYPE = "Type";
    #endregion

    #region " --> CONFIG_SECTION_TAGS                     "
    /// <summary>
    /// Konfigurationsabschnitt für die Tags des HealthChecks.
    /// </summary>
    private const String CONFIG_SECTION_TAGS = "Tags";
    #endregion

    #endregion

    #region " Variablen/ Properties                       "

    #region " --> Name                                    "
    /// <summary>
    /// Name des HealthChecks.
    /// </summary>
    public String Name
    {
      get => this._Name.ToString();
      init => this._Name = value.AsMemory();
    }
    private ReadOnlyMemory<Char> _Name;
    #endregion

    #region " --> Description                             "
    /// <summary>
    /// Beschreibung des HealthChecks.
    /// </summary>
    public String Description
    {
      get => this._Description.ToString();
      set => this._Description = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _Description;
    #endregion

    #region " --> Type                                    "
    /// <summary>
    /// Typ des HealthChecks.
    /// </summary>
    public String Type
    {
      get => this._Type.ToString();
      init => this._Type = value.AsMemory();
    }
    private ReadOnlyMemory<Char> _Type;
    #endregion

    #region " --> Tags                                    "
    /// <summary>
    /// Liste der zugehörigen Tags für den HealthCheck.
    /// </summary>
    public List<String> Tags { get; set; }
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Init                                    "
    /// <summary>
    /// Initialisiert die HealthCheckSettings-Instanz mit Werten aus der angegebenen Konfigurationssektion.
    /// </summary>
    /// <param name="section">Die Konfigurationssektion mit den HealthCheck-Einstellungen.</param>
    /// <returns>Die initialisierte Instanz von <see cref="HealthCheckSettings"/>.</returns>
    public abstract HealthCheckSettings Init(IConfigurationSection section);
    #endregion

    #endregion

    #region " interne Methoden                            "

    #region " --> Bind                                    "
    /// <summary>
    /// Bindet die Werte aus der Konfigurationssektion an die Eigenschaften dieser Instanz.
    /// </summary>
    /// <param name="section">Die Konfigurationssektion mit den HealthCheck-Einstellungen.</param>
    protected virtual void Bind(IConfigurationSection section)
    {
      this._Name = section?.GetValue<String>(CONFIG_VALUE_NAME).AsMemory() ?? ReadOnlyMemory<Char>.Empty;
      this._Description = section?.GetValue<String>(CONFIG_VALUE_DESCRIPTION).AsMemory() ?? ReadOnlyMemory<Char>.Empty;
      this._Type = section?.GetValue<String>(CONFIG_VALUE_TYPE).AsMemory() ?? ReadOnlyMemory<Char>.Empty;
      this.Tags = [];
      section.GetSection(CONFIG_SECTION_TAGS)?.GetChildren().ToArray().ForAll(t => this.Tags.Add(t.Value));
    }
    #endregion

    #endregion

  }
}

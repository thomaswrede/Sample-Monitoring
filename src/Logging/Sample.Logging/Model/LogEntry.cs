using System;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Logging;

namespace Sample.Logging.Model
{
  /// <summary>
  /// Repräsentiert einen einzelnen Logeintrag mit allen relevanten Informationen für die Protokollierung.
  /// </summary>
  public class LogEntry : IUnique
  {
    #region " Variablen/ Properties                       "

    #region " --> Id                                      "
    /// <summary>
    /// Eindeutige Kennung des Logeintrags.
    /// </summary>
    [JsonPropertyName(nameof(Id))]
    public Guid Id { get; internal set; }
    #endregion

    #region " --> Message                                 "
    /// <summary>
    /// Die eigentliche Lognachricht.
    /// </summary>
    [JsonPropertyName(nameof(Message))]
    public String Message
    {
      get => this._Message.ToString();
      internal set => this._Message = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _Message;
    #endregion

    #region " --> ServerName                              "
    /// <summary>
    /// Name des Servers, auf dem der Logeintrag erzeugt wurde.
    /// </summary>
    [JsonPropertyName(nameof(ServerName))]
    public String ServerName
    {
      get => this._ServerName.ToString();
      internal set => this._ServerName = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _ServerName;
    #endregion

    #region " --> ServerIp                                "
    /// <summary>
    /// IP-Adresse des Servers, auf dem der Logeintrag erzeugt wurde.
    /// </summary>
    [JsonPropertyName(nameof(ServerIp))]
    public String ServerIp
    {
      get => this._ServerIp.ToString();
      internal set => this._ServerIp = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _ServerIp;
    #endregion

    #region " --> ObjectData                              "
    /// <summary>
    /// Optionale zusätzliche Objektdaten, die mit dem Logeintrag verknüpft sind.
    /// </summary>
    [JsonPropertyName(nameof(ObjectData))]
    public String ObjectData
    {
      get => this._ObjectData.ToString();
      internal set => this._ObjectData = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _ObjectData;
    #endregion

    #region " --> Identity                                "
    /// <summary>
    /// Identität (z.B. Benutzername), die mit dem Logeintrag assoziiert ist.
    /// </summary>
    [JsonPropertyName(nameof(Identity))]
    public String Identity
    {
      get => this._Identity.ToString();
      internal set => this._Identity = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _Identity;
    #endregion

    #region " --> LogDate                                 "
    /// <summary>
    /// Zeitstempel, wann der Logeintrag erstellt wurde.
    /// </summary>
    [JsonPropertyName(nameof(LogDate))]
    public DateTime LogDate { get; internal set; } = DateTime.Now;
    #endregion

    #region " --> ThreadId                                "
    /// <summary>
    /// Die ID des Threads, in dem der Logeintrag erzeugt wurde (optional).
    /// </summary>
    [JsonPropertyName(nameof(ThreadId))]
    public Int32? ThreadId { get; internal set; }
    #endregion

    #region " --> StackTrace                              "
    /// <summary>
    /// StackTrace-Informationen, falls eine Ausnahme protokolliert wurde.
    /// </summary>
    [JsonPropertyName(nameof(StackTrace))]
    public String StackTrace
    {
      get => this._StackTrace.ToString();
      internal set => this._StackTrace = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _StackTrace;
    #endregion

    #region " --> Source                                  "
    /// <summary>
    /// Quelle des Logeintrags (z.B. Klassen- oder Methodennamen).
    /// </summary>
    [JsonPropertyName(nameof(Source))]
    public String Source
    {
      get => this._Source.ToString();
      internal set => this._Source = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _Source;
    #endregion

    #region " --> Target                                  "
    /// <summary>
    /// Zielobjekt oder -komponente, auf die sich der Logeintrag bezieht.
    /// </summary>
    [JsonPropertyName(nameof(Target))]
    public String Target
    {
      get => this._Target.ToString();
      internal set => this._Target = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _Target;
    #endregion

    #region " --> Exception                               "
    /// <summary>
    /// Ausnahmeinformationen, falls eine Exception protokolliert wurde.
    /// </summary>
    [JsonPropertyName(nameof(Exception))]
    public String Exception
    {
      get => this._Exception.ToString();
      internal set => this._Exception = value?.AsMemory() ?? default;
    }
    private ReadOnlyMemory<Char> _Exception;
    #endregion

    #region " --> LogLevel                                "
    /// <summary>
    /// Das Schweregrad-Level des Logeintrags.
    /// </summary>
    [JsonPropertyName(nameof(LogLevel))]
    public LogLevel LogLevel { get; internal set; }
    #endregion

    #endregion
  }
}

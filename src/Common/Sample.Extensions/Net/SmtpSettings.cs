namespace System.Net
{
  /// <summary>
  /// Stellt SMTP-Konfigurationseinstellungen bereit, wie Hostname, Port und SSL-Aktivierung.
  /// </summary>
  public sealed class SmtpSettings
  {
    #region " Variablen/ Properties                       "

    #region " --> Hostname                                "
    /// <summary>
    /// Ruft den Hostnamen des SMTP-Servers ab oder legt ihn fest.
    /// </summary>
    public String Hostname
    {
      get => this._Hostname.ToString();
      set => this._Hostname = value.AsMemory();
    }
    private ReadOnlyMemory<Char> _Hostname;
    #endregion

    #region " --> Port                                    "
    /// <summary>
    /// Ruft den Port des SMTP-Servers ab oder legt ihn fest.
    /// </summary>
    public Int32? Port { get; set; }
    #endregion

    #region " --> EnableSsl                               "
    /// <summary>
    /// Gibt an, ob SSL für die Verbindung zum SMTP-Server aktiviert werden soll.
    /// </summary>
    public Boolean? EnableSsl { get; set; }
    #endregion

    #endregion

    #region " Konstruktor                                 "

    #region " --> New                                     "
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="SmtpSettings"/>-Klasse.
    /// </summary>
    public SmtpSettings() { }

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="SmtpSettings"/>-Klasse mit Hostname, Port und SSL-Option.
    /// </summary>
    /// <param name="hostname">Der Hostname des SMTP-Servers.</param>
    /// <param name="port">Der Port des SMTP-Servers.</param>
    /// <param name="enableSsl">Gibt an, ob SSL aktiviert werden soll.</param>
    public SmtpSettings(String hostname, Int32? port = default, Boolean? enableSsl = default) : this(hostname.AsMemory(), port, enableSsl) { }

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="SmtpSettings"/>-Klasse mit Hostname als <see cref="ReadOnlyMemory{Char}"/>, Port und SSL-Option.
    /// </summary>
    /// <param name="hostname">Der Hostname des SMTP-Servers als <see cref="ReadOnlyMemory{Char}"/>.</param>
    /// <param name="port">Der Port des SMTP-Servers.</param>
    /// <param name="enableSsl">Gibt an, ob SSL aktiviert werden soll.</param>
    public SmtpSettings(ReadOnlyMemory<Char> hostname, Int32? port = default, Boolean? enableSsl = default)
    {
      this._Hostname = hostname;
      this.Port = port;
      this.EnableSsl = enableSsl;
    }
    #endregion

    #endregion

  }
}

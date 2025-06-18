using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using LogEntry = Sample.Logging.Model.LogEntry;

namespace System.Diagnostics
{
  /// <summary>
  /// Stellt Methoden zum Protokollieren von Informationen, Fehlern und Debugging-Nachrichten bereit.
  /// </summary>
  public static class Log
  {
    #region " Variablen/ Properties                       "

    #region " --> HasHandler                              "
    /// <summary>
    /// Gibt an, ob ein Log-Handler registriert ist.
    /// </summary>
    private static Boolean _HasHandler;
    #endregion

    #region " --> Buffer                                  "
    /// <summary>
    /// Interner Puffer für Logeinträge. Führt die Handler-Aktion aus, wenn der Puffer voll ist oder das Timeout erreicht wird.
    /// </summary>
    private static readonly BufferedList<LogEntry> _Buffer = new(1000, 5000, (entries) =>
    {
      if (_HasHandler)
      {
        _Handle(entries);
      }
    });
    #endregion

    #region " --> Handle                                  "
    /// <summary>
    /// Setzt die Handler-Aktion, die ausgeführt wird, wenn Logeinträge verarbeitet werden sollen.
    /// </summary>
    internal static Action<IEnumerable<LogEntry>> Handle
    {
      set
      {
        _Handle = value;
        _HasHandler = true;
      }
    }
    /// <summary>
    /// Interne Referenz auf die Handler-Aktion für Logeinträge.
    /// </summary>
    private static Action<IEnumerable<LogEntry>> _Handle = (_) => { };
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Debug                                   "
    /// <summary>
    /// Protokolliert eine Debugging-Nachricht.
    /// </summary>
    /// <param name="context">Der Kontext der Nachricht.</param>
    /// <param name="message">Die Nachricht.</param>
    /// <param name="data">Zusätzliche Daten zur Nachricht.</param>
    public static void Debug(
      IContext context,
      ReadOnlyMemory<Char> message = default,
      IDictionary<String, ReadOnlyMemory<Char>> data = default) => _ = _Buffer.TryAdd(new()
      {
        Id = Guid.NewGuid(),
        Message = message.ToString(),
        ThreadId = Environment.CurrentManagedThreadId,
        ServerName = Environment.MachineName,
        ServerIp = Array.Find(Net.Dns.GetHostAddresses(Net.Dns.GetHostName()), e => e.AddressFamily == Net.Sockets.AddressFamily.InterNetwork)?.ToString(),
        ObjectData = data.AggregateToString("#", ":"),
        LogLevel = LogLevel.Debug,
        Identity = context?.Identity?.Name
      });
    #endregion

    #region " --> Info                                    "
    /// <summary>
    /// Protokolliert eine Informationsnachricht.
    /// </summary>
    /// <param name="context">Der Kontext der Nachricht.</param>
    /// <param name="message">Die Nachricht.</param>
    /// <param name="data">Zusätzliche Daten zur Nachricht.</param>
    public static void Info(
      IContext context,
      ReadOnlyMemory<Char> message = default,
      IDictionary<String, ReadOnlyMemory<Char>> data = default) => _ = _Buffer.TryAdd(new()
      {
        Id = Guid.NewGuid(),
        Message = message.ToString(),
        ThreadId = Environment.CurrentManagedThreadId,
        ServerName = Environment.MachineName,
        ServerIp = Array.Find(Net.Dns.GetHostAddresses(Net.Dns.GetHostName()), e => e.AddressFamily == Net.Sockets.AddressFamily.InterNetwork)?.ToString(),
        ObjectData = data.AggregateToString("#", ":"),
        LogLevel = LogLevel.Information,
        Identity = context?.Identity?.Name
      });
    #endregion

    #region " --> Error                                   "
    /// <summary>
    /// Protokolliert eine Fehlermeldung.
    /// </summary>
    /// <param name="context">Der Kontext der Fehlermeldung.</param>
    /// <param name="exception">Die Ausnahme, die den Fehler verursacht hat.</param>
    /// <param name="message">Die Fehlermeldung.</param>
    /// <param name="data">Zusätzliche Daten zur Fehlermeldung.</param>
    public static void Error(
      IContext context,
      Exception exception = default,
      ReadOnlyMemory<Char> message = default,
      IDictionary<String, ReadOnlyMemory<Char>> data = default) => _Buffer.TryAdd(new()
      {
        Id = Guid.NewGuid(),
        Message = message.ToString(),
        ThreadId = Environment.CurrentManagedThreadId,
        ServerName = Environment.MachineName,
        ServerIp = Array.Find(Net.Dns.GetHostAddresses(Net.Dns.GetHostName()), e => e.AddressFamily == Net.Sockets.AddressFamily.InterNetwork)?.ToString(),
        ObjectData = exception.GetExceptionData(data),
        StackTrace = exception.StackTrace,
        Source = exception.Source,
        Target = exception?.TargetSite?.ToString(),
        Exception = exception.ToDetailedString(),
        LogLevel = LogLevel.Error,
        Identity = context?.Identity?.Name
      });
    #endregion

    #endregion

  }
}

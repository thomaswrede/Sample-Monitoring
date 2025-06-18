using System.Collections.Concurrent;
using System.Diagnostics.Tracing;

namespace System.Diagnostics
{
  /// <summary>
  /// Stellt eine EventSource für das Logging von Ausnahmen und Fehlerzählern bereit.
  /// </summary>
  public sealed class LogEventSource : EventSource
  {
    #region " Konstanten                                  "

    #region " --> EVENT_CATEGORY_ERROR                    "
    /// <summary>
    /// Die Kategorie für Fehlerereignisse.
    /// </summary>
    public const String EVENT_CATEGORY_ERROR = "Sample.Exceptions";
    #endregion

    #region " --> EVENT_COUNTER_EXCEPTION_PER_SECOND      "
    /// <summary>
    /// Der Name des EventCounters für ausgelöste Fehler pro Sekunde.
    /// </summary>
    public const String EVENT_COUNTER_EXCEPTION_PER_SECOND = "ausgelöste Fehler/s";
    #endregion

    #region " --> EVENT_COUNTER_NON_CATEGORIZED_EXCEPTION_PER_SECOND"
    /// <summary>
    /// Der Name des EventCounters für ausgelöste nicht kategorisierte Fehler pro Sekunde.
    /// </summary>
    public const String EVENT_COUNTER_NON_CATEGORIZED_EXCEPTION_PER_SECOND = "ausgelöste nicht kategorisierte Fehler/s";
    #endregion

    #endregion

    #region " Variablen/ Properties                       "

    #region " --> HandledExceptions                       "
    /// <summary>
    /// Liste der bereits behandelten Ausnahmen.
    /// </summary>
    private static readonly ConcurrentList<Exception> _HandledExceptions = [];
    #endregion

    #region " --> GenericCounters                         "
    /// <summary>
    /// Dictionary für generische EventCounter, die nach Namen indiziert sind.
    /// </summary>
    private static readonly ConcurrentDictionary<ReadOnlyMemory<Char>, IncrementingEventCounter> _GenericCounters = [];
    #endregion

    #region " --> LogEventSource                          "
    /// <summary>
    /// Singleton-Instanz der LogEventSource.
    /// </summary>
    private static readonly LogEventSource _LogEventSource = new();
    #endregion

    #region " --> ExceptionThrownCounter                  "
    /// <summary>
    /// EventCounter für ausgelöste Ausnahmen.
    /// </summary>
    private IncrementingEventCounter _ExceptionThrownCounter;
    #endregion

    #region " --> NonCategorizedExceptionThrownCounter    "
    /// <summary>
    /// EventCounter für ausgelöste nicht kategorisierte Ausnahmen.
    /// </summary>
    private IncrementingEventCounter _NonCategorizedExceptionThrownCounter;
    #endregion

    #endregion

    #region " Konstruktor                                 "

    #region " --> New                                     "
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="LogEventSource"/>-Klasse.
    /// </summary>
    private LogEventSource() : base(EVENT_CATEGORY_ERROR) { }
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> ExceptionThrown                         "
    /// <summary>
    /// Erhöht den Zähler für ausgelöste Ausnahmen.
    /// </summary>
    public static void ExceptionThrown() => _LogEventSource._ExceptionThrownCounter?.Increment();
    #endregion

    #region " --> NonCategorizedExceptionThrown           "
    /// <summary>
    /// Erhöht den Zähler für nicht kategorisierte Ausnahmen, sofern die Ausnahme nicht bereits behandelt wurde.
    /// </summary>
    /// <param name="ex">Die ausgelöste Ausnahme.</param>
    public static void NonCategorizedExceptionThrown(Exception ex)
    {
      if (!_HandledExceptions.TryRemove(ex))
      {
        _LogEventSource._NonCategorizedExceptionThrownCounter?.Increment();
      }
    }
    #endregion

    #region " --> CategorizedExceptionThrown              "
    /// <summary>
    /// Erhöht den Zähler für eine kategorisierte Ausnahme, sofern die Ausnahme nicht bereits behandelt wurde.
    /// </summary>
    /// <param name="ex">Die ausgelöste Ausnahme.</param>
    /// <param name="counterName">Der Name des zu verwendenden Counters.</param>
    public static void CategorizedExceptionThrown(Exception ex, ReadOnlyMemory<Char> counterName)
    {
      if (ex == default || _HandledExceptions.TryAdd(ex))
      {
        if (!_GenericCounters.TryGetValue(counterName, out IncrementingEventCounter counter))
        {
          counter = _LogEventSource.RegisterCounter(counterName);
        }
        counter?.Increment();
      }
    }
    #endregion

    #region " --> Init                                    "
    /// <summary>
    /// Initialisiert die EventSource und aktiviert die EventCounter.
    /// </summary>
    public static void Init() =>
      //bewirkt, das von außen die Initialisierung und somit das Enable der EventCounter erfolgen kann
      _LogEventSource.Initialize();
    #endregion

    #endregion

    #region " interne Methoden                            "

    #region " --> Initialize                              "
    /// <summary>
    /// Initialisiert die internen EventCounter.
    /// </summary>
    private void Initialize()
    {
      this._ExceptionThrownCounter ??= new IncrementingEventCounter(EVENT_COUNTER_EXCEPTION_PER_SECOND, this)
      {
        DisplayName = Sample.Extensions.Properties.ErrorMessages.EVENT_COUNTER_EXCEPTION_PER_SECOND_DESCRIPTION
      };

      this._NonCategorizedExceptionThrownCounter ??= new IncrementingEventCounter(EVENT_COUNTER_NON_CATEGORIZED_EXCEPTION_PER_SECOND, this)
      {
        DisplayName = Sample.Extensions.Properties.ErrorMessages.EVENT_COUNTER_NON_CATEGORIZED_EXCEPTION_PER_SECOND_DESCRIPTION
      };

      this._ExceptionThrownCounter?.Increment(0.0d);
      this._NonCategorizedExceptionThrownCounter?.Increment(0.0d);
    }
    #endregion

    #region " --> OnEventCommand                          "
    /// <summary>
    /// Reagiert auf EventSource-Befehle und initialisiert ggf. die Counter.
    /// </summary>
    /// <param name="command">Die EventCommandEventArgs-Instanz.</param>
    protected override void OnEventCommand(EventCommandEventArgs command)
    {
      if (command.Command == EventCommand.Enable)
      {
        this.Initialize();
      }
    }
    #endregion

    #region " --> RegisterCounter                         "
    /// <summary>
    /// Registriert einen neuen EventCounter mit dem angegebenen Namen.
    /// </summary>
    /// <param name="counterName">Der Name des Counters.</param>
    /// <returns>Der registrierte <see cref="IncrementingEventCounter"/> oder <c>null</c>, falls bereits vorhanden.</returns>
    private IncrementingEventCounter RegisterCounter(ReadOnlyMemory<Char> counterName)
    {
      IncrementingEventCounter counter = new(counterName.ToString(), this)
      {
        DisplayName = counterName.ToString()
      };
      if (_GenericCounters.TryAdd(counterName, counter))
      {
        counter.Increment(0.0d);
        return counter;
      }
      return default;
    }
    #endregion

    #endregion

  }
}

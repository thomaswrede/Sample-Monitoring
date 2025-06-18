using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System
{
  /// <summary>
  /// Definiert das Ergebnis einer allgemeinen Operation.
  /// </summary>
  public interface IResult
  {
    #region " Properties                                  "

    #region " --> State                                   "
    /// <summary>
    /// Gibt den allgemeinen Status der Operation zurück.
    /// </summary>
    ResultState State { get; }
    #endregion

    #region " --> Message                                 "
    /// <summary>
    /// Gibt eine Nachricht zum Operationsergebnis zurück.
    /// </summary>
    String Message { get; }
    #endregion

    #region " --> Exception                               "
    /// <summary>
    /// Gibt ggf. die Exception zurück, die während der Operation aufgetreten ist.
    /// </summary>
    Exception Exception { get; }
    #endregion

    #region " --> Sink                                    "
    /// <summary>
    /// Gibt das Quellobjekt der Nachricht zurück.
    /// </summary>
    Object Sink { get; }
    #endregion

    #region " --> Results                                 "
    /// <summary>
    /// Gibt die Ergebnisse der einzelnen Unteroperationen als schreibgeschützte Auflistung zurück.
    /// </summary>
    ReadOnlyCollection<IResult> Results { get; }
    #endregion

    #region " --> Parent                                  "
    /// <summary>
    /// Gibt das übergeordnete Operationsergebnis zurück oder legt dieses fest.
    /// </summary>
    IResult Parent { get; set; }
    #endregion

    #region " --> AdditionalData                          "
    /// <summary>
    /// Gibt zusätzliche Daten zum Operationsergebnis zurück.
    /// </summary>
    IDictionary<String, ReadOnlyMemory<Char>> AdditionalData { get; }
    #endregion

    #endregion

    #region " Methoden                                    "

    #region " --> Add                                     "
    /// <summary>
    /// Fügt ein neues Operationsergebnis mit Status und Nachricht hinzu.
    /// </summary>
    /// <param name="sink">Das Quellobjekt der Nachricht.</param>
    /// <param name="state">Der Status der Operation.</param>
    /// <param name="message">Die Nachricht zum Operationsergebnis.</param>
    /// <param name="additionalData">Zusätzliche Daten zum Operationsergebnis.</param>
    void Add(Object sink, ResultState state, ReadOnlyMemory<Char> message = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default);

    /// <summary>
    /// Fügt ein neues Operationsergebnis mit Exception und Nachricht hinzu.
    /// </summary>
    /// <param name="sink">Das Quellobjekt der Nachricht.</param>
    /// <param name="exception">Die aufgetretene Exception.</param>
    /// <param name="message">Die Nachricht zum Operationsergebnis.</param>
    /// <param name="additionalData">Zusätzliche Daten zum Operationsergebnis.</param>
    void Add(Object sink, Exception exception, ReadOnlyMemory<Char> message = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default);

    /// <summary>
    /// Fügt ein bestehendes Operationsergebnis hinzu.
    /// </summary>
    /// <param name="result">Das hinzuzufügende Operationsergebnis.</param>
    void Add(IResult result);
    #endregion

    #endregion
  }

  /// <summary>
  /// Definiert das Ergebnis einer allgemeinen Operation mit Rückgabewert.
  /// </summary>
  /// <typeparam name="T">Typ des Rückgabewerts.</typeparam>
  public interface IResult<T> : IResult
  {
    #region " Properties                                  "

    #region " --> Data                                    "
    /// <summary>
    /// Gibt die Daten des Operationsergebnisses zurück oder legt diese fest.
    /// </summary>
    T Data { get; set; }
    #endregion

    #endregion

  }
}

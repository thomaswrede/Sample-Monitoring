using System.Threading;

namespace System.Collections.Generic
{
  /// <summary>
  /// Eine gepufferte Liste, die Elemente in einem internen Puffer speichert und eine Aktion ausführt, wenn der Puffer eine bestimmte Größe erreicht.
  /// </summary>
  /// <typeparam name="T">Der Typ der Elemente in der Liste.</typeparam>
  public sealed class BufferedList<T> : ICollection<T>, IDisposable
  {
    #region " Variablen/ Properties                       "

    #region " --> SyncRoot                                "
    /// <summary>
    /// Ein Objekt, das zum Synchronisieren des Zugriffs auf die Liste verwendet wird.
    /// </summary>
    private readonly Lock _SyncRoot = new();
    #endregion

    #region " --> IsDisposed                              "
    /// <summary>
    /// Gibt an, ob die Liste bereits entsorgt wurde.
    /// </summary>
    private Boolean _IsDisposed;
    #endregion

    #region " --> InternalList                            "
    /// <summary>
    /// Die interne Liste, in der die Elemente gespeichert werden.
    /// </summary>
    private readonly Concurrent.ConcurrentList<T> _InternalList = [];
    #endregion

    #region " --> BufferSize                              "
    /// <summary>
    /// Die Größe des Puffers. Wenn die Anzahl der Elemente in der Liste diese Größe erreicht, wird die angegebene Aktion ausgeführt.
    /// </summary>
    public Int32 BufferSize { get; set; } = 20;
    #endregion

    #region " --> Count                                   "
    /// <summary>
    /// Gibt die Anzahl der Elemente in der Liste zurück.
    /// </summary>
    public Int32 Count => this._InternalList.Count;
    #endregion

    #region " --> IsReadOnly                              "
    /// <summary>
    /// Gibt an, ob die Liste schreibgeschützt ist. Immer false für diese Implementierung.
    /// </summary>
    public Boolean IsReadOnly => false;
    #endregion

    #region " --> ReachedAction                           "
    /// <summary>
    /// Die Aktion, die ausgeführt wird, wenn die Anzahl der Elemente in der Liste die Puffergröße erreicht.
    /// </summary>
    private readonly Action<IEnumerable<T>> _ReachedAction;
    #endregion

    #region " --> ItemCount                               "
    /// <summary>
    /// Die aktuelle Anzahl der Elemente in der Liste.
    /// </summary>
    private Int32 _ItemCount;
    #endregion

    #region " --> ReachedTimer                            "
    /// <summary>
    /// Ein Timer, der verwendet wird, um die Aktion auszuführen, wenn die Anzahl der Elemente in der Liste die Puffergröße erreicht.
    /// </summary>
    private Timers.Timer _ReachedTimer;
    #endregion

    #endregion

    #region " Konstruktor/ Destruktor                     "

    #region " --> New                                     "
    /// <summary>
    /// Erstellt eine neue gepufferte Liste mit der angegebenen Puffergröße und Aktion.
    /// </summary>
    /// <param name="bufferSize">Die Größe des Puffers.</param>
    /// <param name="reachedAction">Die Aktion, die ausgeführt wird, wenn die Anzahl der Elemente in der Liste die Puffergröße erreicht.</param>
    public BufferedList(Int32 bufferSize, Action<IEnumerable<T>> reachedAction)
    {
      this.BufferSize = bufferSize;
      this._ReachedAction = reachedAction;
    }
    /// <summary>
    /// Erstellt eine neue gepufferte Liste mit der angegebenen Puffergröße, Timeout und Aktion.
    /// </summary>
    /// <param name="bufferSize">Die Größe des Puffers.</param>
    /// <param name="timeout">Das Timeout für den Timer.</param>
    /// <param name="reachedAction">Die Aktion, die ausgeführt wird, wenn die Anzahl der Elemente in der Liste die Puffergröße erreicht.</param>
    public BufferedList(Int32 bufferSize, Int32 timeout, Action<IEnumerable<T>> reachedAction)
    {
      this.BufferSize = bufferSize;
      this._ReachedAction = reachedAction;
      this._ReachedTimer = new Timers.Timer(timeout);
      this._ReachedTimer.Elapsed += this.ReachedTimer_Elapsed;
    }
    #endregion

    #region " --> Dispose                                 "
    /// <summary>
    /// Gibt alle Ressourcen frei, die von der Liste verwendet werden.
    /// </summary>
    public void Dispose()
    {
      if (this._IsDisposed) { return; }
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }
    private void Dispose(Boolean disposing)
    {
      if (this._IsDisposed) { return; }
      if (disposing)
      {
        if (this._ReachedTimer != default)
        {
          this._ReachedTimer.Elapsed -= this.ReachedTimer_Elapsed;
          this._ReachedTimer.Dispose();
          this._ReachedTimer = default;
        }
      }
      this._IsDisposed = true;
    }
    #endregion

    #endregion

    #region " EventHandler                                "

    #region " --> ReachedTimer_Elapsed                    "
    /// <summary>
    /// Wird aufgerufen, wenn der Timer abgelaufen ist. Führt die angegebene Aktion aus.
    /// </summary>
    private void ReachedTimer_Elapsed(Object sender, Timers.ElapsedEventArgs e) => this.BufferReached();
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Contains                                "
    /// <summary>
    /// Bestimmt, ob die Liste ein bestimmtes Element enthält.
    /// </summary>
    /// <param name="item">Das zu suchende Element.</param>
    /// <returns>True, wenn das Element in der Liste gefunden wurde, sonst false.</returns>
    public Boolean Contains(T item) => this._InternalList.Contains(item);
    #endregion

    #region " --> CopyTo                                  "
    /// <summary>
    /// Kopiert die Elemente der Liste in ein Array, beginnend bei einem bestimmten Array-Index.
    /// </summary>
    /// <param name="array">Das Array, in das die Elemente kopiert werden sollen.</param>
    /// <param name="arrayIndex">Der Index im Array, an dem das Kopieren beginnen soll.</param>
    public void CopyTo(T[] array, Int32 arrayIndex) => this._InternalList.CopyTo(array, arrayIndex);
    #endregion

    #region " --> GetEnumerator                           "
    /// <summary>
    /// Gibt einen Enumerator zurück, der die Liste durchläuft.
    /// </summary>
    /// <returns>Ein Enumerator, der die Liste durchläuft.</returns>
    public IEnumerator<T> GetEnumerator() => this._InternalList.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this._InternalList.GetEnumerator();
    #endregion

    #region " --> Add                                     "
    /// <summary>
    /// Fügt ein Element zur Liste hinzu.
    /// </summary>
    /// <param name="item">Das hinzuzufügende Element.</param>
    public void Add(T item)
    {
      if (this._ItemCount == 0 && this._ReachedTimer != default) { this._ReachedTimer.Start(); }
      this._InternalList.Add(item);
      this._ItemCount++;
      this.CheckBuffer();
    }
    #endregion

    #region " --> TryAdd                                  "
    /// <summary>
    /// Versucht, ein Element zur Liste hinzuzufügen.
    /// </summary>
    /// <param name="item">Das hinzuzufügende Element.</param>
    /// <returns>True, wenn das Element erfolgreich hinzugefügt wurde, sonst false.</returns>
    public Boolean TryAdd(T item)
    {
      Boolean result;
      if (this._ItemCount == 0 && this._ReachedTimer != default) { this._ReachedTimer.Start(); }

      result = this._InternalList.TryAdd(item);
      this._ItemCount++;
      this.CheckBuffer();

      return result;
    }
    #endregion

    #region " --> Clear                                   "
    /// <summary>
    /// Entfernt alle Elemente aus der Liste.
    /// </summary>
    public void Clear()
    {
      if (this._ReachedTimer != default) { this._ReachedTimer.Stop(); }
      this._InternalList.Clear();
      this._ItemCount = 0;
    }
    #endregion

    #region " --> Remove                                  "
    /// <summary>
    /// Entfernt das erste Vorkommen eines bestimmten Elements aus der Liste.
    /// </summary>
    /// <param name="item">Das zu entfernende Element.</param>
    /// <returns>True, wenn das Element erfolgreich entfernt wurde, sonst false.</returns>
    public Boolean Remove(T item)
    {
      if (this._InternalList.Remove(item))
      {
        this._ItemCount--;
        if (this._ItemCount == 0 && this._ReachedTimer != default) { this._ReachedTimer.Stop(); }
        return true;
      }

      return false;
    }
    #endregion

    #endregion

    #region " interne Methoden                            "

    #region " --> CheckBuffer                             "
    /// <summary>
    /// Überprüft, ob die Anzahl der Elemente in der Liste die Puffergröße erreicht hat, und führt gegebenenfalls die angegebene Aktion aus.
    /// </summary>
    private void CheckBuffer()
    {
      if (this._ItemCount >= this.BufferSize)
      {
        this.BufferReached();
      }
    }
    #endregion

    #region " --> BufferReached                           "
    /// <summary>
    /// Wird aufgerufen, wenn die Anzahl der Elemente in der Liste die Puffergröße erreicht hat. Führt die angegebene Aktion aus.
    /// </summary>
    private void BufferReached()
    {
      lock (this._SyncRoot)
      {
        if (this._InternalList.Count != 0)
        {
          T[] items = new T[this._InternalList.Count];
          this._InternalList.CopyTo(items, 0);
          this._InternalList.Clear();
          this._ItemCount = 0;
          this._ReachedAction(items);
          if (this._ReachedTimer != default) { this._ReachedTimer.Stop(); }
        }
      }
    }
    #endregion

    #endregion

  }
}

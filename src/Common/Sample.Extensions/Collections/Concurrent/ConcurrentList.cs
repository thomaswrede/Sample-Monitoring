using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Collections.Concurrent
{
  /// <summary>
  /// Stellt eine threadsichere Auflistung dar, auf die durch mehrere Threads gleichzeitig zugegriffen werden kann.
  /// </summary>
  /// <typeparam name="T">Der Typ der Werte in der Auflistung</typeparam>
  public sealed class ConcurrentList<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
  {
    #region " Variablen/ Properties                       "

    #region " --> SyncRoot                                "
    Object ICollection.SyncRoot => throw new NotSupportedException();
    private readonly Lock _SyncRoot = new();
    #endregion

    #region " --> List                                    "
    private readonly List<T> _List;
    #endregion

    #region " --> Count                                   "
    /// <summary>
    /// Ruft die Anzahl der Werte in der ConcurrentList ab.
    /// </summary>
    public Int32 Count => this._List.Count;
    #endregion

    #region " --> IsReadOnly                              "
    /// <summary>
    /// Ruft einen Wert ab, der angibt, ob die IList schreibgeschützt ist.
    /// </summary>
    public Boolean IsReadOnly => false;
    #endregion

    #region " --> IsSynchronized                          "
    /// <summary>
    /// Ruft einen Wert ab, der angibt, ob der Zugriff auf die ICollection mit SyncRoot synchronisiert wird.
    /// </summary>
    public Boolean IsSynchronized => false;
    #endregion

    #endregion

    #region " Konstruktor                                 "

    #region " --> New                                     "
    /// <summary>
    /// Initialisiert eine neue, leere Instanz der ConcurrentList-Klasse mit der Standardparallelitätsebene und der Standardanfangskapazität, wobei der Standardvergleich für den Werttyp verwendet wird.
    /// </summary>
    public ConcurrentList() => this._List = [];
    /// <summary>
    /// Initialisiert eine neue Instanz der ConcurrentList-Klasse, die aus dem angegebenen IEnumerable kopierte Elemente enthält, die Standardparallelitätsebene und Standardanfangskapazität aufweist und den angegebenen IEqualityComparer<T> verwendet.
    /// </summary>
    /// <param name="collection"></param>
    public ConcurrentList(IEnumerable<T> collection) => this._List = [.. collection];
    #endregion

    #endregion

    #region " öffentlich Methoden                         "

    #region " --> TryAdd                                  "
    /// <summary>
    /// Versucht, der ConcurrentList den angegebenen Wert hinzuzufügen.
    /// </summary>
    /// <param name="item">Der Wert des hinzuzufügenden Elements. Der Wert kann für Verweistypen null sein.</param>
    /// <returns>true, wenn der Wert zur ConcurrentList<T> hinzugefügt wurde, false, wenn der Wert bereits vorhanden ist.</returns>
    public Boolean TryAdd(T item)
    {
      lock (this._SyncRoot)
      {
        this._List.Add(item);
      }

      return true;
    }
    #endregion

    #region " --> TryRemove                               "
    /// <summary>
    /// Versucht, den Wert aus der ConcurrentList zu entfernen.
    /// </summary>
    /// <param name="item">Der Wert des Elements, das entfernt werden soll.</param>
    /// <returns>true, wenn das Objekt erfolgreich entfernt wurde, andernfalls false.</returns>
    public Boolean TryRemove(T item)
    {
      Boolean success = false;
      lock (this._SyncRoot)
      {
        if (this._List.Contains(item))
        {
          _ = this._List.Remove(item);
          success = true;
        }
      }

      return success;
    }
    #endregion

    #region " --> Add                                     "
    /// <summary>
    /// Fügt der ConcurrentList das angegebene Element hinzu.
    /// </summary>
    /// <param name="item">Das hinzuzufügende Element.</param>
    public void Add(T item) => _ = this.TryAdd(item);
    #endregion

    #region " --> Clear                                   "
    /// <summary>
    /// Entfernt sämtliche Werte aus der ConcurrentList.
    /// </summary>
    public void Clear()
    {
      lock (this._SyncRoot)
      {
        this._List.Clear();
      }
    }
    #endregion

    #region " --> Remove                                  "
    public Boolean Remove(T item) => this.TryRemove(item);
    #endregion

    #region " --> CopyTo                                  "
    /// <summary>
    /// Kopiert die Elemente der ICollection in ein Array, wobei am angegebenen Arrayindex begonnen wird.
    /// </summary>
    /// <param name="array">Das eindimensionale Array, dass das Ziel der aus ICollection kopierten Elemente ist. Für das Array muss eine nullbasierte Indizierung verwendet werden.</param>
    /// <param name="arrayIndex">Der nullbasierte Index im array, bei dem der Kopiervorgang beginnt.</param>
    /// <exception cref="ArgumentNullException">array ist null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">index ist kleiner als 0.</exception>
    /// <exception cref="ArgumentException">index ist größer oder gleich der Länge von array.
    /// - oder -
    /// Die Anzahl der Elemente in der Quell-ICollection ist größer als der verfügbare Platz vom index bis zum Ende des Ziel-arrays.
    /// </exception>
    public void CopyTo(T[] array, Int32 arrayIndex)
    {
      T[] items = default;
      lock (this._SyncRoot)
      {
        items = [.. this._List.ToArray().Take(array.Length - arrayIndex)];
      }
      items.CopyTo(array, arrayIndex);
    }
    /// <summary>
    /// Kopiert die Elemente der ICollection in ein Array, beginnend bei einem bestimmten Array-Index.
    /// </summary>
    /// <param name="array">Das eindimensionale Array, das Ziel der aus der ICollection kopierten Elemente ist. Für das Array muss eine nullbasierte Indizierung verwendet werden.</param>
    /// <param name="index">Der nullbasierte Index im array, bei dem der Kopiervorgang beginnt.</param>
    /// <exception cref="ArgumentNullException">array ist null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">index ist kleiner als 0.</exception>
    /// <exception cref="ArgumentException">array ist mehrdimensional.
    /// - oder -
    /// Die Anzahl der Elemente in der Quell-ICollection ist größer als der verfügbare Platz vom index bis zum Ende des Ziel-arrays.
    /// - oder -
    /// Der Typ der Quell-ICollection kann nicht automatisch in den Typ des Ziel-array umgewandelt werden.
    /// </exception>
    public void CopyTo(Array array, Int32 index)
    {
      T[] items = default;
      lock (this._SyncRoot)
      {
        items = [.. this._List];
      }

      items.CopyTo(array, index);
    }
    #endregion

    #region " --> Contains                                "
    public Boolean Contains(T item) => this._List.Contains(item); //rein theoretisch hier kein Lock erforderlich      
    #endregion

    #region " --> GetEnumerator                           "
    public IEnumerator<T> GetEnumerator()
    {
      List<T> items = default;
      lock (this._SyncRoot)
      {
        items = [.. this._List];
      }

      return items.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
      T[] items = default;
      lock (this._SyncRoot)
      {
        items = [.. this._List];
      }

      return items.GetEnumerator();
    }
    #endregion

    #endregion

  }
}

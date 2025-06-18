using System.Linq;

namespace System.Collections.Generic
{
  public static class ExtensionsIList
  {
    #region " öffentliche Methoden                        "

    #region " --> AddOrUpdate                             "
    /// <summary>
    /// Fügt einen neuen Wert hinzu oder aktualisiert einen vorhandenen Wert in der Liste.
    /// </summary>
    /// <typeparam name="TValue">Der Typ des Wertes.</typeparam>
    /// <typeparam name="TKey">Der Typ des Schlüssels.</typeparam>
    /// <param name="list">Die Liste, in welcher der Wert hinzugefügt oder aktualisiert werden soll.</param>
    /// <param name="newKey">Der Schlüssel des hinzuzufügenden oder zu aktualisierenden Werts.</param>
    /// <param name="newValue">Der hinzuzufügende oder zu aktualisierende Wert.</param>
    /// <param name="keyPredicate">Die Funktion, die den Schlüssel aus dem Wert extrahiert.</param>
    /// <returns>Der hinzugefügte oder aktualisierte Wert.</returns>
    public static TValue AddOrUpdate<TValue, TKey>(this IList<TValue> list, TKey newKey, TValue newValue, Func<TValue, TKey> keyPredicate) where TKey : IEquatable<TKey>
    {
      if (list == default) { return default; }
      TValue value = list.FirstOrDefault(e => newKey.Equals(keyPredicate(e)));
      if (value == null)
      {
        list.Add(newValue);
      }
      else
      {
        Int32 index = list.IndexOf(value);
        list[index] = newValue;
      }

      return newValue;
    }
    #endregion

    #region " --> TryAdd                                  "
    /// <summary>
    /// Versucht, einen Wert zur Liste hinzuzufügen.
    /// </summary>
    /// <typeparam name="TValue">Der Typ des hinzuzufügenden Werts.</typeparam>
    /// <param name="list">Die Liste, zu welcher der Wert hinzugefügt werden soll.</param>
    /// <param name="item">Der hinzuzufügende Wert.</param>
    /// <returns>True, wenn der Wert erfolgreich hinzugefügt wurde, sonst False.</returns>
    public static Boolean TryAdd<TValue>(this IList<TValue> list, TValue item)
    {
      if (list == default
        || item == null
        || list.Contains(item)) { return false; }

      try
      {
        list.Add(item);
        return true;
      }
      catch
      {
        return false;
      }
    }
    #endregion

    #endregion

  }
}

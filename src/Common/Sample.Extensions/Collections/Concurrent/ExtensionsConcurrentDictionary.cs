using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Concurrent
{
  /// <summary>
  /// Stellt Erweiterungsmethoden für <see cref="ConcurrentDictionary{TKey, TValue}"/> bereit.
  /// </summary>
  public static class ExtensionsConcurrentDictionary
  {
    #region " öffentliche Methoden                        "

    #region " --> Add                                     "
    /// <summary>
    /// Fügt dem <see cref="ConcurrentDictionary{TKey, TValue}"/> ein neues Element mit dem angegebenen Schlüssel und Wert hinzu.
    /// </summary>
    /// <typeparam name="TKey">Der Typ des Schlüssels.</typeparam>
    /// <typeparam name="TValue">Der Typ des Werts.</typeparam>
    /// <param name="dictionary">Das zu erweiternde <see cref="ConcurrentDictionary{TKey, TValue}"/>.</param>
    /// <param name="key">Der Schlüssel des hinzuzufügenden Elements.</param>
    /// <param name="value">Der Wert des hinzuzufügenden Elements.</param>
    public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
      if (dictionary == default) { return; }
      _ = dictionary.TryAdd(key, value);
    }

    /// <summary>
    /// Fügt dem <see cref="ConcurrentDictionary{TKey, TValue}"/> ein neues Element anhand eines <see cref="KeyValuePair{TKey, TValue}"/> hinzu.
    /// </summary>
    /// <typeparam name="TKey">Der Typ des Schlüssels.</typeparam>
    /// <typeparam name="TValue">Der Typ des Werts.</typeparam>
    /// <param name="dictionary">Das zu erweiternde <see cref="ConcurrentDictionary{TKey, TValue}"/>.</param>
    /// <param name="value">Das hinzuzufügende <see cref="KeyValuePair{TKey, TValue}"/>.</param>
    public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> value)
    {
      if (dictionary == default) { return; }
      _ = dictionary.TryAdd(value.Key, value.Value);
    }

    /// <summary>
    /// Fügt dem <see cref="ConcurrentDictionary{TKey, TValue}"/> mehrere Elemente aus einer Auflistung von <see cref="KeyValuePair{TKey, TValue}"/> hinzu.
    /// </summary>
    /// <typeparam name="TKey">Der Typ des Schlüssels.</typeparam>
    /// <typeparam name="TValue">Der Typ des Werts.</typeparam>
    /// <param name="dictionary">Das zu erweiternde <see cref="ConcurrentDictionary{TKey, TValue}"/>.</param>
    /// <param name="items">Die hinzuzufügenden Elemente als Auflistung von <see cref="KeyValuePair{TKey, TValue}"/>.</param>
    public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
      if (dictionary == default) { return; }
      items.ForAll(x => dictionary.Add(x.Key, x.Value));
    }
    #endregion

    #endregion
  }
}

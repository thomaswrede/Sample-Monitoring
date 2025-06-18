using System.Text;

namespace System.Collections.Generic
{
  public static class ExtensionsIDictionary
  {
    #region " öffentliche Methoden                        "

    #region " --> AggregateToString                       "
    /// <summary>
    /// Wandelt die Sequenz in eine Zeichenkette um, indem sie die Schlüssel und Werte mit dem angegebenen Trennzeichen verbindet.
    /// </summary>
    /// <param name="sequence">Die Sequenz, die in eine Zeichenkette umgewandelt werden soll.</param>
    /// <param name="keySeperator">Das Trennzeichen, das zwischen Schlüssel und Wert eingefügt werden soll.</param>
    /// <param name="entrySeperator">Das Trennzeichen, das zwischen den einzelnen Einträgen eingefügt werden soll.</param>
    /// <returns>Gibt eine Zeichenkette zurück, welche die Schlüssel und Werte der Sequenz enthält, getrennt durch das angegebene Trennzeichen.</returns>
    public static String AggregateToString(this IDictionary sequence, String keySeperator, String entrySeperator)
    {
      if (sequence == default
        || sequence.Count == 0)
      {
        return String.Empty;
      }
      IDictionaryEnumerator enumerator = sequence.GetEnumerator();

      StringBuilder returnValue = new();
      while (true)
      {
        Object key = enumerator.MoveNext() ? enumerator.Key : default;
        if (key == null) { break; }

        _ = returnValue.Append(key.ToString());
        _ = returnValue.Append(keySeperator);
        _ = returnValue.Append(enumerator.Value == default ? String.Empty : enumerator.Value.ToString());
        _ = returnValue.Append(entrySeperator);
      }

      return returnValue.ToString();
    }
    /// <summary>
    /// Wandelt die Sequenz in eine Zeichenkette um, indem sie die Schlüssel und Werte mit dem angegebenen Trennzeichen verbindet.
    /// </summary>
    /// <param name="sequence">Die Sequenz, die in eine Zeichenkette umgewandelt werden soll.</param>
    /// <param name="keySeperator">Das Trennzeichen, das zwischen Schlüssel und Wert eingefügt werden soll.</param>
    /// <param name="entrySeperator">Das Trennzeichen, das zwischen den einzelnen Einträgen eingefügt werden soll.</param>
    /// <returns>Gibt eine Zeichenkette zurück, welche die Schlüssel und Werte der Sequenz enthält, getrennt durch das angegebene Trennzeichen.</returns>
    public static String AggregateToString<TKey, TValue>(this IDictionary<TKey, TValue> sequence, String keySeperator, String entrySeperator)
    {
      if (sequence == default
        || sequence.Count == 0)
      {
        return String.Empty;
      }
      IEnumerator<KeyValuePair<TKey, TValue>> enumerator = sequence.GetEnumerator();

      StringBuilder returnValue = new();
      while (true)
      {
        Object key = enumerator.MoveNext() ? enumerator.Current.Key : default;
        if (key == null) { break; }

        _ = returnValue.Append(key.ToString());
        _ = returnValue.Append(keySeperator);
        _ = returnValue.Append(enumerator.Current.Value == null ? String.Empty : enumerator.Current.Value.ToString());
        _ = returnValue.Append(entrySeperator);
      }

      return returnValue.ToString();
    }
    /// <summary>
    /// Wandelt die Sequenz in eine Zeichenkette um, indem sie die Schlüssel und Werte mit dem angegebenen Trennzeichen verbindet.
    /// </summary>
    /// <param name="sequence">Die Sequenz, die in eine Zeichenkette umgewandelt werden soll.</param>
    /// <param name="keySeperator">Das Trennzeichen, das zwischen Schlüssel und Wert eingefügt werden soll.</param>
    /// <param name="entrySeperator">Das Trennzeichen, das zwischen den einzelnen Einträgen eingefügt werden soll.</param>
    /// <returns>Gibt eine Zeichenkette zurück, welche die Schlüssel und Werte der Sequenz enthält, getrennt durch das angegebene Trennzeichen.</returns>
    public static String AggregateToString<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> sequence, String keySeperator, String entrySeperator)
    {
      if (sequence == default
        || sequence.Count == 0)
      {
        return String.Empty;
      }
      IEnumerator<KeyValuePair<TKey, TValue>> enumerator = sequence.GetEnumerator();

      StringBuilder returnValue = new();
      while (true)
      {
        Object key = enumerator.MoveNext() ? enumerator.Current.Key : default;
        if (key == null) { break; }

        _ = returnValue.Append(key.ToString());
        _ = returnValue.Append(keySeperator);
        _ = returnValue.Append(enumerator.Current.Value == null ? String.Empty : enumerator.Current.Value.ToString());
        _ = returnValue.Append(entrySeperator);
      }

      return returnValue.ToString();
    }
    #endregion

    #region " --> TryAddValue                             "
    /// <summary>
    /// Versucht, den angegebenen Schlüssel und Wert zur Sammlung hinzuzufügen.
    /// </summary>
    /// <param name="collection">Die Sammlung, zu welcher der Schlüssel und Wert hinzugefügt werden sollen.</param>
    /// <param name="key">Der Schlüssel, der hinzugefügt werden soll.</param>
    /// <param name="value">Der Wert, der hinzugefügt werden soll.</param>
    /// <returns>Gibt true zurück, wenn der Schlüssel und Wert erfolgreich hinzugefügt wurden, sonst false.</returns>
    public static Boolean TryAddValue<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, TValue value)
    {
      if (collection == default
        || key == null
        || collection.ContainsKey(key)) { return false; }
      else
      {
        try
        {
          collection.Add(key, value);
          return true;
        }
        catch
        {
          return false;
        }
      }
    }
    #endregion

    #region " --> GetValueOrFallback                      "
    /// <summary>
    /// Gibt den Wert für den angegebenen Schlüssel zurück. Wenn der Schlüssel nicht gefunden wird, wird ein Standardwert zurückgegeben.
    /// </summary>
    /// <param name="value">Das Wörterbuch, aus dem der Wert abgerufen werden soll.</param>
    /// <param name="key">Der Schlüssel des zu suchenden Werts.</param>
    /// <param name="fallback">Der Standardwert, der zurückgegeben wird, wenn der Schlüssel nicht gefunden wird.</param>
    /// <returns>Der Wert für den angegebenen Schlüssel, oder der Standardwert, wenn der Schlüssel nicht gefunden wird.</returns>
    public static TValue GetValueOrFallback<TKey, TValue>(this IDictionary<TKey, TValue> value, TKey key, TValue fallback) => value == default
        || key == null
        ? fallback
        : value.TryGetValue(key, out TValue val) ? val : fallback;

    #endregion

    #region " --> TryRemove                               "
    /// <summary>
    /// Versucht, den Wert mit dem angegebenen Schlüssel zu entfernen.
    /// </summary>
    /// <param name="collection">Die Sammlung, aus welcher der Wert entfernt werden soll.</param>
    /// <param name="key">Der Schlüssel des zu entfernenden Werts.</param>
    /// <param name="value">Wenn diese Methode zurückgegeben wird, enthält sie den entfernten Wert, wenn der Schlüssel gefunden wurde, oder den Standardwert des Typs, wenn der Schlüssel nicht gefunden wurde.</param>
    /// <returns>true, wenn der Wert erfolgreich entfernt wurde; andernfalls false.</returns>
    public static Boolean TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, out TValue value)
    {
      if (collection == default
        || collection.Count == 0)
      {
        value = default;
        return false;
      }

      return collection.TryGetValue(key, out value) && collection.Remove(key);
    }
    #endregion

    #region " --> Add                                     "
    /// <summary>
    /// Fügt der Sammlung einen Schlüssel und einen Wert hinzu, wenn bestimmte Bedingungen erfüllt sind.
    /// </summary>
    /// <param name="collection">Die Sammlung, zu welcher der Schlüssel und Wert hinzugefügt werden sollen.</param>
    /// <param name="key">Der hinzuzufügende Schlüssel.</param>
    /// <param name="value">Der hinzuzufügende Wert.</param>
    /// <param name="skipOnEmpty">Wenn true, wird der Wert nicht hinzugefügt, wenn er leer ist.</param>
    public static void Add(this IDictionary<String, String> collection, String key, String value, Boolean skipOnEmpty)
    {
      if (collection == default) { return; }
      if (skipOnEmpty && String.IsNullOrEmpty(value)) { return; }
      collection.Add(key, value);
    }
    #endregion

    #region " --> AddOrUpdate                             "
    /// <summary>
    /// Fügt der Liste einen Schlüssel und einen Wert hinzu oder aktualisiert den Wert, wenn der Schlüssel bereits vorhanden ist.
    /// </summary>
    /// <param name="list">Die Liste, zu welcher der Schlüssel und Wert hinzugefügt oder in der sie aktualisiert werden sollen.</param>
    /// <param name="key">Der hinzuzufügende oder zu aktualisierende Schlüssel.</param>
    /// <param name="value">Der hinzuzufügende oder zu aktualisierende Wert.</param>
    /// <returns>Gibt den hinzugefügten oder aktualisierten Wert zurück.</returns>
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key, TValue value)
    {
      if (list == default) { return default; }
      if (list.ContainsKey(key))
      {
        list[key] = value;
      }
      else
      {
        list.Add(key, value);
      }

      return value;
    }
    #endregion

    #endregion

  }
}

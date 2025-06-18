using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
  public static class ExtensionsIEnumerable
  {
    #region " Variablen/ Properties                       "

    #region " --> OrderByExpressions                      "
    private static readonly Dictionary<Int32, Delegate> _OrderByExpressions = [];
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Contains                                "
    /// <summary>
    /// Überprüft, ob ein bestimmter Wert in der Quelle vorhanden ist und gibt die Position zurück
    /// </summary>
    /// <typeparam name="TSource">Typen-Platzhalter</typeparam>
    /// <param name="source">Quelle, die durchsucht werden soll</param>
    /// <param name="value">Wert, der gesucht wird</param>
    /// <param name="position">Position des gesuchten Werts in der Quelle</param>
    /// <returns>Gibt 'true' zurück, wenn der Wert gefunden wurde, sonst 'false'</returns>
    public static Boolean Contains<TSource>(this IEnumerable<TSource> source, TSource value, out Int32 position)
    {
      if (source == default
        || value == null)
      {
        position = -1;
        return false;
      }
      IEnumerator<TSource> enumerator = source.GetEnumerator();
      Int32 index = -1;
      while (enumerator.MoveNext())
      {
        index++;
        if (value.Equals(enumerator.Current))
        {
          position = index;
          return true;
        }
      }
      position = -1;
      return false;
    }
    #endregion

    #region " --> AggregateToString                       "
    /// <summary>
    /// Arbeitet eine Auflistung durch und setzt daraus einen
    /// String zusammen und gibt diesen zurück
    /// </summary>
    /// <param name="sequence">zu verarbeitende Auflistung</param>
    /// <param name="seperator">Trennzeichen</param>
    /// <returns>zusammmengesetzte Zeichenkette</returns>
    public static String AggregateToString(this IEnumerable<String> sequence, String seperator)
    {
      if (!(sequence?.Any() ?? false)) { return String.Empty; }

      StringBuilder returnValue = new();
      Boolean first = true;
      sequence.ForAll(e =>
      {
        if (!first) { _ = returnValue.Append(seperator); }
        else { first = false; }
        _ = returnValue.Append(e);
      });

      return returnValue.ToString();
    }
    #endregion

    #region " --> CountAll                                "
    /// <summary>
    /// Zählt sämtliche Einträge in einer Auflistung
    /// und gibt die Anzahl als Int32 zurück
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <returns>Anzahl der Einträge(Int32)</returns>
    public static Int32 CountAll<T>(this IEnumerable<T> value)
    {
      if (value == default) { return 0; }

      Int32 count = 0;

      IEnumerator<T> enumerator = value.GetEnumerator();
      while (enumerator.MoveNext())
      {
        count++;
      }

      return count;
    }
    #endregion

    #region " --> ForAll/Async                            "
    /// <summary>
    /// Geht eine Auflistung durch und führt für jeden
    /// Eintrag eine Funktion durch
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="action">auszuführende Funktion</param>
    public static void ForAll<T>(this IEnumerable<T> value, Action<T> action)
    {
      if (value == default) { return; }
      if (action == default) { return; }

      IEnumerator<T> enumerator = value.GetEnumerator();

      while (enumerator.MoveNext())
      {
        action.Invoke(enumerator.Current);
      }
      //foreach (T item in value)
      //{
      //  action(item);
      //}
    }
    public static async Task ForAllAsync<T>(this IEnumerable<T> value, Action<T> action)
    {
      if (value == default) { return; }
      if (action == default) { return; }

      IEnumerator<T> enumerator = value.GetEnumerator();

      while (enumerator.MoveNext())
      {
        await Task.Run(() => action(enumerator.Current));
      }

      //foreach (T item in value)
      //{
      //  await Task.Run(() => action(item));
      //}
    }
    public static async Task ForAllAsync<T>(this Task<IEnumerable<T>> value, Action<T> action)
    {
      if (value == default) { return; }
      if (action == default) { return; }

      foreach (T item in await value)
      {
        await Task.Run(() => action(item));
      }
    }
    public static async Task ForAllAsync<T>(this IAsyncEnumerable<T> value, Action<T> action)
    {
      if (value == default) { return; }
      if (action == default) { return; }

      IAsyncEnumerator<T> enumerator = value.GetAsyncEnumerator();

      while (await enumerator.MoveNextAsync())
      {
        action(enumerator.Current);
      }
    }
    public static async Task ForAllAsync<T>(this IEnumerable<T> value, Action<T> action, Action<T, Exception> failure)
    {
      if (value == default) { return; }
      if (action == default) { return; }

      foreach (T item in value)
      {
        try
        {
          await Task.Run(() => action(item));
        }
        catch (Exception ex)
        {
          await Task.Run(() => failure(item, ex));
        }
      }
    }
    /// <summary>
    /// Geht eine Auflistung durch und fügt jeden
    /// Eintrag mit einer Funktion einem Result-Objekt
    /// hinzu und gibt dieses zurück
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <typeparam name="TResult">Result-Objekt</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="action">auszuführende Funktion</param>
    /// <returns>Result-Objekt</returns>
    public static IEnumerable<TResult> ForAll<T, TResult>(this IEnumerable<T> value, Func<T, TResult> action)
    {
      if (value == default) { return default; }
      if (action == default) { return default; }

      List<TResult> result = [];
      foreach (T item in value)
      {
        result.Add(action(item));
      }

      return result;
    }
    public static async Task<IEnumerable<TResult>> ForAllAsync<T, TResult>(this IEnumerable<T> value, Func<T, Task<TResult>> action)
    {
      if (value == default
        || action == default) { return await Task.FromResult<IEnumerable<TResult>>(default); }

      List<TResult> result = [];
      foreach (T item in value)
      {
        result.Add(await action(item));
      }

      return result;
    }
    /// <summary>
    /// Geht eine Auflistung durch und führt für jeden
    /// Eintrag eine Funktion und im Fehlerfall eine andere
    /// Funktion durch
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="action">auszuführende Funktion</param>
    /// <param name="failure">auszuführende Funktion im Fehlerfall</param>
    public static void ForAll<T>(this IEnumerable<T> value, Action<T> action, Action<T, Exception> failure)
    {
      if (value == default) { return; }
      if (action == default) { return; }

      foreach (T item in value)
      {
        try
        {
          action(item);
        }
        catch (Exception ex)
        {
          if (failure != default) { failure(item, ex); }
        }
      }
    }
    /// <summary>
    /// Geht eine Auflistung durch und fügt jeden
    /// Eintrag mit einer Funktion einem Result-Objekt
    /// hinzu und gibt dieses zurück
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <typeparam name="TResult">Result-Objekt</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="action">auszuführende Funktion</param>
    /// <param name="commitAction"></param>
    /// <returns>Result-Objekt</returns>
    public static IEnumerable<TResult> ForAll<T, TResult>(this IEnumerable<T> value, Func<T, TResult> action, Func<TResult, TResult> commitAction)
    {
      if (value == default
        || action == default) { return default; }

      List<TResult> transaction = [];
      List<TResult> result = [];
      foreach (T item in value)
      {
        TResult entry = action(item);
        transaction.Add(entry);
      }

      foreach (TResult item in transaction)
      {
        TResult entry = commitAction != default ? commitAction(item) : item;
        result.Add(entry);
      }

      return result;
    }
    /// <summary>
    /// Geht eine Auflistung durch und fügt jeden
    /// Eintrag, eine Funktion und im Fehlerfall eine andere
    /// Funktion einem Result-Objekt hinzu und gibt
    /// dieses zurück
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <typeparam name="TResult">Result-Objekt</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="action">auszuführende Funktion</param>
    /// <param name="failure">auszuführende Funktion im Fehlerfall</param>
    /// <returns>Result-Objekt</returns>
    public static IEnumerable<TResult> ForAll<T, TResult>(this IEnumerable<T> value, Func<T, TResult> action, Func<T, Exception, TResult> failure)
    {
      if (value == default
        || action == default) { return default; }

      List<TResult> result = [];
      foreach (T item in value)
      {
        try
        {
          TResult entry = action(item);
          result.Add(entry);
        }
        catch (Exception ex)
        {
          if (failure != default)
          {
            TResult entry = failure(item, ex);
            result.Add(entry);
          }
        }
      }

      return result;
    }
    #endregion

    #region " --> ForAllRecursive                         "
    /// <summary>
    /// Geht eine Auflistung rekursiv durch und führt für jeden
    /// Eintrag eine Funktion durch
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="selector"></param>
    /// <param name="action">auszuführende Funktion</param>
    public static void ForAllRecursive<T>(this IEnumerable<T> value, Func<T, IEnumerable<T>> selector, Action<T> action)
    {
      if (value == default
        || selector == default
        || action == default) { return; }

      foreach (T item in value)
      {
        if (item != null)
        {
          action(item);
          selector(item).ForAllRecursive(selector, action);
        }
      }
    }
    /// <summary>
    /// Geht eine Auflistung Rekursiv durch und fügt jeden
    /// Eintrag mit einer Funktion einem Result-Objekt
    /// hinzu und gibt dieses zurück
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <typeparam name="TResult">Result-Objekt</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="selector"></param>
    /// <param name="action">auszuführende Funktion</param>
    /// <returns>Result-Objekt</returns>
    public static IEnumerable<TResult> ForAllRecursive<T, TResult>(this IEnumerable<T> value, Func<T, IEnumerable<T>> selector, Func<T, TResult> action)
    {
      if (value == default
        || selector == default
        || action == default) { return default; }

      List<TResult> result = [];
      foreach (T item in value)
      {
        result.Add(action(item));
        result.AddRange(selector(item).ForAllRecursive(selector, action));
      }

      return result;
    }
    /// <summary>
    /// Geht eine Auflistung Rekursiv durch und führt für jeden
    /// Eintrag eine Funktion und im Fehlerfall eine andere
    /// Funktion durch
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="selector"></param>
    /// <param name="action">auszuführende Funktion</param>
    /// <param name="failure">auszuführende Funktion im Fehlerfall</param>
    public static void ForAllRecursive<T>(this IEnumerable<T> value, Func<T, IEnumerable<T>> selector, Action<T> action, Action<T, Exception> failure)
    {
      if (value == default
        || selector == default
        || action == default) { return; }

      foreach (T item in value)
      {
        try
        {
          action(item);
          selector(item).ForAllRecursive(selector, action, failure);
        }
        catch (Exception ex)
        {
          if (failure != default) { failure(item, ex); }
        }
      }
    }
    /// <summary>
    /// Geht eine Auflistung Rekursiv durch und fügt jeden
    /// Eintrag, eine Funktion und im Fehlerfall eine andere
    /// Funktion einem Result-Objekt hinzu und gibt
    /// dieses zurück
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <typeparam name="TResult">Result-Objekt</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="selector"></param>
    /// <param name="action">auszuführende Funktion</param>
    /// <param name="failure">auszuführende Funktion im Fehlerfall</param>
    /// <returns>Result-Objekt</returns>
    public static IEnumerable<TResult> ForAllRecursive<T, TResult>(this IEnumerable<T> value, Func<T, IEnumerable<T>> selector, Func<T, TResult> action, Func<T, Exception, TResult> failure)
    {
      if (value == default
        || selector == default
        || action == default) { return default; }

      List<TResult> result = [];
      foreach (T item in value)
      {
        try
        {
          result.Add(action(item));
          result.AddRange(selector(item).ForAllRecursive(selector, action, failure));
        }
        catch (Exception ex)
        {
          if (failure != default) { result.Add(failure(item, ex)); }
        }
      }

      return result;
    }
    #endregion

    #region " --> SelectUntil                             "
    /// <summary>
    /// schreibt Einträge aus einer Auflistung zurück
    /// solange eine Bedingung TRUE liefert
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="action">auszuführende Funktion</param>
    /// <param name="condition">Bedingung für die Ausführung</param>
    /// <returns>schreibt die einzelnen Einträge zurück</returns>
    public static IEnumerable<T> SelectUntil<T>(this IEnumerable<T> value, Action<T> action, Func<T, Boolean> condition)
    {
      if (value == default) { yield break; }

      IEnumerator<T> enumerator = value.GetEnumerator();

      T item;
      do
      {
        if (!enumerator.MoveNext()) { break; }
        item = enumerator.Current;
        if (action != default) { action(item); }
        yield return item;
      }
      while (condition(item));
    }
    #endregion

    #region " --> Exists                                  "
    /// <summary>
    /// Prüft ob <paramref name="predicate"/> in der Auflistung
    /// <paramref name="value"/> vorhanden ist und gibt
    /// TRUE oder FALSE zurück
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="predicate"></param>
    /// <returns>TRUE oder FALSE</returns>
    public static Boolean Exists<T>(this IEnumerable<T> value, Func<T, Boolean> predicate)
    {
      if (value == default
        || predicate == default) { return false; }
      foreach (T item in value)
      {
        if (predicate(item)) { return true; }
      }

      return false;
    }
    #endregion

    #region " --> MoveUp                                  "
    /// <summary>
    /// Bewegt einen Eintrag innerhalb einer Auflistung
    /// um 1 nach oben
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="item">zu verschiebender Eintrag</param>
    /// <returns>TRUE oder FALSE</returns>
    public static Boolean MoveUp<T>(this IEnumerable<T> value, T item)
    {
      if (value == default) { return false; }
      IList<T> list = value as IList<T>;
      if (list == default) { return false; }

      Int32 oldIndex = list.IndexOf(item);
      if (oldIndex == 0) { return false; }

      T swapElement = list[oldIndex - 1];
      list[oldIndex - 1] = item;
      list[oldIndex] = swapElement;
      return true;
    }
    #endregion

    #region " --> MoveDown                                "
    /// <summary>
    /// Bewegt einen Eintrag innerhalb einer Auflistung
    /// um 1 nach unten
    /// </summary>
    /// <typeparam name="T">Typen-Platzhalter</typeparam>
    /// <param name="value">zu verarbeitende Auflistung</param>
    /// <param name="item">zu verschiebender Eintrag</param>
    /// <returns>TRUE oder FALSE</returns>
    public static Boolean MoveDown<T>(this IEnumerable<T> value, T item)
    {
      if (value == default) { return false; }
      IList<T> list = value as IList<T>;
      if (list == default) { return false; }

      Int32 oldIndex = list.IndexOf(item);
      if (oldIndex == list.Count - 1) { return false; }

      T swapElement = list[oldIndex + 1];
      list[oldIndex + 1] = item;
      list[oldIndex] = swapElement;
      return true;
    }
    #endregion

    #region " --> OrderBy                                 "
    //public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, String propertyName)
    //{
    //  if (source == default) { return default; }

    //  ParameterExpression param = Expression.Parameter(typeof(T));
    //  MemberExpression memberAccess = Expression.Property(param, propertyName);
    //  UnaryExpression convertedMemberAccess = Expression.Convert(memberAccess, typeof(Object));
    //  Expression<Func<T, Object>> orderPredicate = Expression.Lambda<Func<T, Object>>(convertedMemberAccess, param);

    //  Int32 hashKey = orderPredicate.GetHashCode();
    //  if (!_OrderByExpressions.TryGetValue(hashKey, out Delegate orderExpression))
    //  {
    //    orderExpression = orderPredicate.Compile();
    //    _OrderByExpressions.Add(hashKey, orderExpression);
    //  }

    //  return source.AsQueryable().OrderBy((Func<T, Object>)orderExpression);
    //}
    //public static IOrderedAsyncEnumerable<T> OrderBy<T>(this IAsyncEnumerable<T> source, String propertyName)
    //{
    //  if (source == default) { return default; }

    //  ParameterExpression param = Expression.Parameter(typeof(T));
    //  MemberExpression memberAccess = Expression.Property(param, propertyName);
    //  UnaryExpression convertedMemberAccess = Expression.Convert(memberAccess, typeof(Object));
    //  Expression<Func<T, Object>> orderPredicate = Expression.Lambda<Func<T, Object>>(convertedMemberAccess, param);

    //  Int32 hashKey = orderPredicate.GetHashCode();
    //  if (!_OrderByExpressions.TryGetValue(hashKey, out Delegate orderExpression))
    //  {
    //    orderExpression = orderPredicate.Compile();
    //    _OrderByExpressions.Add(hashKey, orderExpression);
    //  }
    //  return source.OrderBy((Func<T, Object>)orderExpression);
    //}
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, String propertyName)
    {
      if (source == default) { return default; }

      ParameterExpression param = Expression.Parameter(typeof(T));
      MemberExpression memberAccess = Expression.Property(param, propertyName);
      UnaryExpression convertedMemberAccess = Expression.Convert(memberAccess, typeof(Object));
      Expression<Func<T, Object>> orderPredicate = Expression.Lambda<Func<T, Object>>(convertedMemberAccess, param);

      Int32 hashKey = orderPredicate.GetHashCode();
      if (!_OrderByExpressions.TryGetValue(hashKey, out Delegate orderExpression))
      {
        orderExpression = orderPredicate.Compile();
        _OrderByExpressions.Add(hashKey, orderExpression);
      }

      return source.OrderBy((Func<T, Object>)orderExpression).AsQueryable();
    }
    #endregion

    #region " --> OrderByDescending                       "
    //public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> source, String propertyName)
    //{
    //  if (source == default) { return default; }

    //  ParameterExpression param = Expression.Parameter(typeof(T));
    //  MemberExpression memberAccess = Expression.Property(param, propertyName);
    //  UnaryExpression convertedMemberAccess = Expression.Convert(memberAccess, typeof(Object));
    //  Expression<Func<T, Object>> orderPredicate = Expression.Lambda<Func<T, Object>>(convertedMemberAccess, param);

    //  Int32 hashKey = orderPredicate.GetHashCode();
    //  if (!_OrderByExpressions.TryGetValue(hashKey, out Delegate orderExpression))
    //  {
    //    orderExpression = orderPredicate.Compile();
    //    _OrderByExpressions.Add(hashKey, orderExpression);
    //  }

    //  return source.AsQueryable().OrderByDescending((Func<T, Object>)orderExpression);
    //}
    //public static IOrderedAsyncEnumerable<T> OrderByDescending<T>(this IAsyncEnumerable<T> source, String propertyName)
    //{
    //  if (source == default) { return default; }

    //  ParameterExpression param = Expression.Parameter(typeof(T));
    //  MemberExpression memberAccess = Expression.Property(param, propertyName);
    //  UnaryExpression convertedMemberAccess = Expression.Convert(memberAccess, typeof(Object));
    //  Expression<Func<T, Object>> orderPredicate = Expression.Lambda<Func<T, Object>>(convertedMemberAccess, param);

    //  Int32 hashKey = orderPredicate.GetHashCode();
    //  if (!_OrderByExpressions.TryGetValue(hashKey, out Delegate orderExpression))
    //  {
    //    orderExpression = orderPredicate.Compile();
    //    _OrderByExpressions.Add(hashKey, orderExpression);
    //  }
    //  return source.OrderByDescending((Func<T, Object>)orderExpression);
    //}
    public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, String propertyName)
    {
      if (source == default) { return default; }

      ParameterExpression param = Expression.Parameter(typeof(T));
      MemberExpression memberAccess = Expression.Property(param, propertyName);
      UnaryExpression convertedMemberAccess = Expression.Convert(memberAccess, typeof(Object));
      Expression<Func<T, Object>> orderPredicate = Expression.Lambda<Func<T, Object>>(convertedMemberAccess, param);

      Int32 hashKey = orderPredicate.GetHashCode();
      if (!_OrderByExpressions.TryGetValue(hashKey, out Delegate orderExpression))
      {
        orderExpression = orderPredicate.Compile();
        _OrderByExpressions.Add(hashKey, orderExpression);
      }

      return source.OrderByDescending((Func<T, Object>)orderExpression).AsQueryable();
    }
    #endregion

    #endregion
  }
}

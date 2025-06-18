using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace System
{
  namespace System
  {
    /// <summary>
    /// Stellt Erweiterungsmethoden für die Typkonvertierung und Null-Prüfung bereit.
    /// </summary>
    public static class ExtensionsObject
    {
      #region " Konstanten                                  "

      #region " --> TypeConversionMap                       "
      /// <summary>
      /// Enthält eine Zuordnung von Typen zu Konvertierungsfunktionen.
      /// </summary>
      private static readonly Dictionary<Type, Func<Object, Object>> _TypeConversionMap = new()
      {
        { Constants.TYPE_STRING, v => v.AsString() },
        { Constants.TYPE_BOOLEAN, v => v.AsBoolean() },
        { Constants.TYPE_INT16, v => v.AsInt16() },
        { Constants.TYPE_INT32, v => v.AsInt32() },
        { Constants.TYPE_INT64, v => v.AsInt64() },
        { Constants.TYPE_DECIMAL, v => v.AsDecimal() },
        { Constants.TYPE_BYTE, v => v.AsByte() },
        { Constants.TYPE_DATETIME, v => v.AsDateTime() },
        { Constants.TYPE_SINGLE, v => v.AsSingle() },
        { Constants.TYPE_DOUBLE, v => v.AsDouble() },
        { Constants.TYPE_GUID, v => v.AsGuid() },
        { Constants.TYPE_BYTES, v => v.AsBytes() },
        { Constants.TYPE_NULLABLE_BOOLEAN, v => v.AsBoolean() },
        { Constants.TYPE_NULLABLE_INT16, v => v.AsInt16() },
        { Constants.TYPE_NULLABLE_INT32, v => v.AsInt32() },
        { Constants.TYPE_NULLABLE_INT64, v => v.AsInt64() },
        { Constants.TYPE_NULLABLE_DECIMAL, v => v.AsDecimal() },
        { Constants.TYPE_NULLABLE_BYTE, v => v.AsByte() },
        { Constants.TYPE_NULLABLE_DATETIME, v => v.AsDateTime() },
        { Constants.TYPE_NULLABLE_SINGLE, v => v.AsSingle() },
        { Constants.TYPE_NULLABLE_DOUBLE, v => v.AsDouble() },
        { Constants.TYPE_NULLABLE_GUID, v => v.AsGuid() },
      };
      #endregion

      #endregion

      #region " öffentliche Methoden                        "

      #region " --> As/Async                                "
      /// <summary>
      /// Wandelt ein Objekt in einen angegebenen Typ um.
      /// </summary>
      /// <param name="value">Das umzuwandelnde Objekt.</param>
      /// <param name="convertTo">Der Zieltyp.</param>
      /// <returns>Das konvertierte Objekt oder <c>default</c>, wenn die Konvertierung fehlschlägt.</returns>
      public static Object As(this Object value, Type convertTo)
      {
        if (value == default
          || convertTo == default) { return default; }

        if (value.GetType() == convertTo) { return value; }

        if (convertTo.IsEnum)
        {
          Type t = Enum.GetUnderlyingType(convertTo);
          if (_TypeConversionMap.TryGetValue(t, out Func<Object, Object> c))
          {
            Object v = c(value);
            return v != default ? Enum.ToObject(convertTo, c(value)) : default;
          }
        }
        else
        {
          if (_TypeConversionMap.TryGetValue(convertTo, out Func<Object, Object> convert)) { return convert(value); }
        }
        return default;
      }

      /// <summary>
      /// Wandelt ein Objekt in den angegebenen generischen Typ <typeparamref name="T"/> um.
      /// </summary>
      /// <typeparam name="T">Der Zieltyp.</typeparam>
      /// <param name="value">Das umzuwandelnde Objekt.</param>
      /// <returns>Das konvertierte Objekt oder <c>default</c>, wenn die Konvertierung fehlschlägt.</returns>
      public static T As<T>(this Object value) => value is default(Object) or null
          ? default
          : _TypeConversionMap.TryGetValue(typeof(T), out Func<Object, Object> convert) ? (T)convert(value) : default;

      /// <summary>
      /// Wandelt ein Objekt asynchron in den angegebenen generischen Typ <typeparamref name="T"/> um.
      /// </summary>
      /// <typeparam name="T">Der Zieltyp.</typeparam>
      /// <param name="value">Das umzuwandelnde Objekt.</param>
      /// <returns>Ein Task mit dem konvertierten Objekt oder <c>default</c>, wenn die Konvertierung fehlschlägt.</returns>
      public static async Task<T> AsAsync<T>(this Object value) => await Task.Run(() => value is default(Object) or null
          ? default
          : _TypeConversionMap.TryGetValue(typeof(T), out Func<Object, Object> convert) ? (T)convert(value) : default);
      #endregion

      #region " --> AsString                                "
      /// <summary>
      /// Wandelt ein Objekt in eine Zeichenkette um und gibt diese
      /// oder einen leeren String zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als String oder ein leerer String.</returns>
      public static String AsString(this Object value)
      {
        if (value == default) { return String.Empty; }
        String s = value as String;
        if (String.IsNullOrEmpty(s))
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString();
          }
        }
        return s;
      }
      #endregion

      #region " --> AsGuid                                  "
      /// <summary>
      /// Wandelt ein Objekt in eine <see cref="Guid?"/> um und gibt diese
      /// oder eine <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Guid oder <c>default</c>.</returns>
      public static Guid? AsGuid(this Object value)
      {
        if (value == default) { return default; }
        Guid? s = value as Guid?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString().AsGuid();
          }
        }
        return s;
      }
      #endregion

      #region " --> AsByte                                  "
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Byte?"/> um und gibt diesen
      /// oder einen <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Byte? oder <c>default</c>.</returns>
      public static Byte? AsByte(this Object value)
      {
        if (value == default) { return default; }
        Byte? s = value as Byte?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString().AsByte();
          }
        }
        return s;
      }
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Byte?"/> um und gibt diesen
      /// oder einen übergebenen Alternativwert zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <param name="defaultValue">Übergebener Alternativwert.</param>
      /// <returns>Objekt als Byte? oder Alternativwert.</returns>
      public static Byte? AsByte(this Object value, Byte? defaultValue) => value.AsByte() ?? defaultValue;
      #endregion

      #region " --> AsBytes                                 "
      /// <summary>
      /// Wandelt ein Objekt in ein <see cref="Byte[]"/> um und gibt dieses zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Byte[].</returns>
      public static Byte[] AsBytes(this Object value)
      {
        if (value == default) { return default; }
        Byte[] s = value as Byte[];
        return s;
      }
      #endregion

      #region " --> AsInt16                                 "
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Int16?"/> um und gibt diesen
      /// oder einen <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Int16? oder <c>default</c>.</returns>
      public static Int16? AsInt16(this Object value)
      {
        if (value == default) { return default; }
        Int16? s = value as Int16?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString().AsInt16();
          }
        }
        return s;
      }
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Int16?"/> um und gibt diesen
      /// oder einen übergebenen Alternativwert zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <param name="defaultValue">Übergebener Alternativwert.</param>
      /// <returns>Objekt als Int16? oder Alternativwert.</returns>
      public static Int16? AsInt16(this Object value, Int16? defaultValue) => value.AsInt16() ?? defaultValue;
      #endregion

      #region " --> AsInt32                                 "
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Int32?"/> um und gibt diesen
      /// oder einen <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Int32? oder <c>default</c>.</returns>
      public static Int32? AsInt32(this Object value)
      {
        if (value == default) { return default; }
        Int32? s = value as Int32?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString().AsInt32();
          }
        }
        return s;
      }
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Int32?"/> um und gibt diesen
      /// oder einen übergebenen Alternativwert zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <param name="defaultValue">Übergebener Alternativwert.</param>
      /// <returns>Objekt als Int32? oder Alternativwert.</returns>
      public static Int32? AsInt32(this Object value, Int32? defaultValue) => value.AsInt32() ?? defaultValue;
      #endregion

      #region " --> AsInt64                                 "
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Int64?"/> um und gibt diesen
      /// oder einen <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Int64? oder <c>default</c>.</returns>
      public static Int64? AsInt64(this Object value)
      {
        if (value == default) { return default; }
        Int64? s = value as Int64?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString().AsInt64();
          }
        }
        return s;
      }
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Int64?"/> um und gibt diesen
      /// oder einen übergebenen Alternativwert zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <param name="defaultValue">Übergebener Alternativwert.</param>
      /// <returns>Objekt als Int64? oder Alternativwert.</returns>
      public static Int64? AsInt64(this Object value, Int64? defaultValue) => value.AsInt64() ?? defaultValue;
      #endregion

      #region " --> AsSingle                                "
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Single?"/> um und gibt diesen
      /// oder einen <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Single? oder <c>default</c>.</returns>
      public static Single? AsSingle(this Object value)
      {
        if (value == default) { return default; }
        Single? s = value as Single?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString(Globalization.CultureInfo.InvariantCulture).AsSingle(invariant: true);
          }
        }
        return s;
      }
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Single?"/> um und gibt diesen
      /// oder einen übergebenen Alternativwert zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <param name="defaultValue">Übergebener Alternativwert.</param>
      /// <returns>Objekt als Single? oder Alternativwert.</returns>
      public static Single? AsSingle(this Object value, Single? defaultValue) => value.AsSingle() ?? defaultValue;
      #endregion

      #region " --> AsDouble                                "
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Double?"/> um und gibt diesen
      /// oder einen <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Double? oder <c>default</c>.</returns>
      public static Double? AsDouble(this Object value)
      {
        if (value == default) { return default; }
        Double? s = value as Double?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString(Globalization.CultureInfo.InvariantCulture).AsDouble(invariant: true);
          }
        }
        return s;
      }
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Double?"/> um und gibt diesen
      /// oder einen übergebenen Alternativwert zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <param name="defaultValue">Übergebener Alternativwert.</param>
      /// <returns>Objekt als Double? oder Alternativwert.</returns>
      public static Double? AsDouble(this Object value, Double? defaultValue) => value.AsDouble() ?? defaultValue;
      #endregion

      #region " --> AsDecimal                               "
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Decimal?"/> um und gibt diesen
      /// oder einen <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Decimal? oder <c>default</c>.</returns>
      public static Decimal? AsDecimal(this Object value)
      {
        if (value == default) { return default; }
        Decimal? s = value as Decimal?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString(Globalization.CultureInfo.InvariantCulture).AsDecimal(invariant: true);
          }
        }
        return s;
      }
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Decimal?"/> um und gibt diesen
      /// oder einen übergebenen Alternativwert zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <param name="defaultValue">Übergebener Alternativwert.</param>
      /// <returns>Objekt als Decimal? oder Alternativwert.</returns>
      public static Decimal? AsDecimal(this Object value, Decimal? defaultValue) => value.AsDecimal() ?? defaultValue;
      #endregion

      #region " --> AsDateTime                              "
      /// <summary>
      /// Wandelt ein Objekt in ein <see cref="DateTime?"/> um und gibt dieses
      /// oder ein <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als DateTime? oder <c>default</c>.</returns>
      public static DateTime? AsDateTime(this Object value)
      {
        if (value == default) { return default; }
        DateTime? s = value as DateTime?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString().AsDateTime();
          }
        }
        return s;
      }
      #endregion

      #region " --> AsTimeSpan                              "
      /// <summary>
      /// Wandelt ein Objekt in eine <see cref="TimeSpan?"/> um und gibt diese
      /// oder eine <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als TimeSpan? oder <c>default</c>.</returns>
      public static TimeSpan? AsTimeSpan(this Object value)
      {
        if (value == default) { return default; }
        TimeSpan? s = value as TimeSpan?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString().AsTimeSpan();
          }
        }
        return s;
      }
      #endregion

      #region " --> AsBoolean                               "
      /// <summary>
      /// Wandelt ein Objekt in einen <see cref="Boolean?"/> um und gibt
      /// diesen oder einen <c>default</c> zurück.
      /// </summary>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Objekt als Boolean? oder <c>default</c>.</returns>
      public static Boolean? AsBoolean(this Object value)
      {
        if (value == default) { return default; }
        Boolean? s = value as Boolean?;
        if (!s.HasValue)
        {
          IConvertible convertable = value as IConvertible;
          if (convertable != default)
          {
            return convertable.ToString().AsBoolean();
          }
        }
        return s;
      }
      #endregion

      #region " --> DownCast                                "
      /// <summary>
      /// Wandelt ein Objekt in einen angegebenen <typeparamref name="TValue"/> Typen
      /// um und gibt diesen zurück.
      /// </summary>
      /// <typeparam name="TValue">Typen-Platzhalter.</typeparam>
      /// <param name="value">Umzuwandelndes Objekt.</param>
      /// <returns>Das Objekt als <typeparamref name="TValue"/> oder <c>default</c> bei Fehler.</returns>
      public static TValue DownCast<TValue>(this Object value)
      {
        if (value == default) { return default; }
        try
        {
          Object castedObject = value.As<TValue>();

          if (castedObject == default)
          {
            Type targetType = typeof(TValue);
            Type valueType = value.GetType();

            Type typeDefinition = targetType.GetGenericTypeDefinition();
            Type genericType = typeDefinition.MakeGenericType(valueType.GenericTypeArguments);

            MethodInfo castMethod = typeof(ExtensionsObject).GetMethod("Cast").MakeGenericMethod(genericType);
            castedObject = castMethod.Invoke(null, [value]);
          }
          return (TValue)castedObject;
        }
        catch
        {
          return default;
        }
      }
      #endregion

      #region " --> Cast                                    "
      /// <summary>
      /// Wandelt ein Objekt in einen angegebenen <typeparamref name="T"/> Typen
      /// um und gibt diesen zurück.
      /// </summary>
      /// <typeparam name="T">Typen-Platzhalter.</typeparam>
      /// <param name="o">Zu castendes Objekt.</param>
      /// <returns>Umgewandeltes Objekt.</returns>
      public static T Cast<T>(Object o) => (T)o;
      #endregion

      #region " --> TryCast                                 "
      /// <summary>
      /// Versucht, ein Objekt in einen angegebenen Typ zu casten.
      /// </summary>
      /// <param name="o">Das zu castende Objekt.</param>
      /// <param name="type">Der Zieltyp.</param>
      /// <param name="result">Das Ergebnis der Konvertierung.</param>
      /// <returns><c>true</c>, wenn das Casten erfolgreich war, sonst <c>false</c>.</returns>
      public static Boolean TryCast(this Object o, Type type, out Object result)
      {
        if (o == default
          || type == default)
        {
          result = default;
          return false;
        }

        if (o.GetType().IsAssignableTo(type))
        {
          result = typeof(ExtensionsObject).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(type).Invoke(null, [o]);
          return true;
        }
        else
        {
          result = o.As(type);
          return result != default;
        }
      }
      #endregion

      #region " --> NotNull/Async                           "
      /// <summary>
      /// Führt eine Aktion aus, wenn das Objekt nicht null ist, andernfalls eine alternative Aktion.
      /// </summary>
      /// <typeparam name="TValue">Typ des Werts.</typeparam>
      /// <param name="value">Zu prüfender Wert.</param>
      /// <param name="notNullAction">Aktion, wenn Wert nicht null.</param>
      /// <param name="nullAction">Aktion, wenn Wert null.</param>
      public static void NotNull<TValue>(this TValue value, Action<TValue> notNullAction, Action nullAction)
      {
        if (nullAction == default) { return; }
        if (notNullAction == default) { return; }

        if (value == null) { nullAction(); }
        else { notNullAction(value); }
      }

      /// <summary>
      /// Führt asynchron eine Aktion aus, wenn das Objekt nicht null ist, andernfalls eine alternative Aktion.
      /// </summary>
      /// <typeparam name="TValue">Typ des Werts.</typeparam>
      /// <param name="value">Zu prüfender Wert.</param>
      /// <param name="notNullActionAsync">Asynchrone Aktion, wenn Wert nicht null.</param>
      /// <param name="nullActionAsync">Asynchrone Aktion, wenn Wert null.</param>
      public static async Task NotNull<TValue>(this TValue value, Func<TValue, Task> notNullActionAsync, Func<Task> nullActionAsync)
      {
        if (nullActionAsync == default) { return; }
        if (notNullActionAsync == default) { return; }

        if (value == null) { await nullActionAsync(); }
        else { await notNullActionAsync(value); }
      }

      /// <summary>
      /// Gibt ein Ergebnis zurück, abhängig davon, ob das Objekt null ist oder nicht.
      /// </summary>
      /// <typeparam name="TValue">Typ des Werts.</typeparam>
      /// <typeparam name="TResult">Typ des Rückgabewerts.</typeparam>
      /// <param name="value">Zu prüfender Wert.</param>
      /// <param name="notNullAction">Funktion, wenn Wert nicht null.</param>
      /// <param name="nullAction">Funktion, wenn Wert null.</param>
      /// <returns>Das Ergebnis der entsprechenden Funktion.</returns>
      public static TResult NotNull<TValue, TResult>(this TValue value, Func<TValue, TResult> notNullAction, Func<TResult> nullAction)
      {
        if (nullAction == default) { nullAction = () => default; }
        if (notNullAction == default) { notNullAction = (_) => default; }
        return value == null ? nullAction() : notNullAction(value);
      }

      /// <summary>
      /// Gibt asynchron ein Ergebnis zurück, abhängig davon, ob das Objekt null ist oder nicht.
      /// </summary>
      /// <typeparam name="TValue">Typ des Werts.</typeparam>
      /// <typeparam name="TResult">Typ des Rückgabewerts.</typeparam>
      /// <param name="value">Zu prüfender Wert.</param>
      /// <param name="notNullActionAsync">Asynchrone Funktion, wenn Wert nicht null.</param>
      /// <param name="nullActionAsync">Asynchrone Funktion, wenn Wert null.</param>
      /// <returns>Ein Task mit dem Ergebnis der entsprechenden Funktion.</returns>
      public static async Task<TResult> NotNullAsync<TValue, TResult>(this TValue value, Func<TValue, Task<TResult>> notNullActionAsync, Func<Task<TResult>> nullActionAsync)
      {
        if (nullActionAsync == default) { nullActionAsync = () => Task.FromResult<TResult>(default); }
        if (notNullActionAsync == default) { notNullActionAsync = (_) => Task.FromResult<TResult>(default); }

        return value == null ? await nullActionAsync() : await notNullActionAsync(value);
      }
      #endregion

      #endregion

    }
  }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace System
{
  /// <summary>
  /// Erweiterungsmethoden für Strings.
  /// </summary>
  public static class ExtensionsString
  {
    #region " öffentliche Methoden                        "

    #region " --> AsGuid                                  "
    /// <summary>
    /// Interpretiert den aktuellen String als Guid und liefert
    /// diese zurück. Ist es nicht möglich den String in eine
    /// Guid zu konvertieren, wird NULL zurück geliefert.
    /// </summary>
    /// <param name="value">Die aktuelle String-Instanz.</param>
    /// <returns>Der String als Guid.</returns>
    public static Guid? AsGuid(this String value, Guid? defaultValue = default) => String.IsNullOrEmpty(value)
       || !Guid.TryParse(value, out Guid result) ? defaultValue : result;
    #endregion

    #region " --> AsByte                                  "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Byte zurück,
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static Byte? AsByte(this String value, Byte? defaultValue = default) => String.IsNullOrEmpty(value) ? defaultValue : Byte.TryParse(value, out Byte result) ? new Byte?(result) : defaultValue;
    #endregion

    #region " --> AsInt16                                 "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Int16 zurück,
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static Int16? AsInt16(this String value, Int16? defaultValue = default) => String.IsNullOrEmpty(value) ? defaultValue : Int16.TryParse(value, out Int16 result) ? new Int16?(result) : defaultValue;
    #endregion

    #region " --> AsInt32                                 "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Int32 zurück,
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static Int32? AsInt32(this String value, Int32? defaultValue = default) => String.IsNullOrEmpty(value) ? defaultValue : Int32.TryParse(value, out Int32 result) ? new Int32?(result) : defaultValue;
    #endregion

    #region " --> AsInt64                                 "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Int64 zurück,
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static Int64? AsInt64(this String value, Int64? defaultValue = default) => String.IsNullOrEmpty(value) ? defaultValue : Int64.TryParse(value, out Int64 result) ? new Int64?(result) : defaultValue;
    #endregion

    #region " --> AsUInt16                                "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Int16 ohne Vorzeichen
    /// zurück, falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static UInt16? AsUInt16(this String value, UInt16? defaultValue = default) => String.IsNullOrEmpty(value) ? defaultValue : UInt16.TryParse(value, out UInt16 result) ? new UInt16?(result) : defaultValue;
    #endregion

    #region " --> AsUInt32                                "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Int32 ohne Vorzeichen
    /// zurück, falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static UInt32? AsUInt32(this String value, UInt32? defaultValue = default) => String.IsNullOrEmpty(value) ? defaultValue : UInt32.TryParse(value, out UInt32 result) ? new UInt32?(result) : defaultValue;
    #endregion

    #region " --> AsUInt64                                "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Int64 ohne Vorzeichen
    /// zurück, falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static UInt64? AsUInt64(this String value, UInt64? defaultValue = default) => String.IsNullOrEmpty(value) ? defaultValue : UInt64.TryParse(value, out UInt64 result) ? new UInt64?(result) : defaultValue;
    #endregion

    #region " --> AsDecimal                               "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Decimal zurück
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="invariant">gibt an, ob ein Sprach-/ Kulturunabhängiges Format verwendet wird</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static Decimal? AsDecimal(this String value, Boolean invariant = true, Decimal? defaultValue = default) => String.IsNullOrEmpty(value)
        ? defaultValue
        : !invariant && Decimal.TryParse(value, NumberStyles.Currency, CultureInfo.CurrentCulture, out Decimal result)
        ? new Decimal?(result)
        : Decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out result) ? new Decimal?(result) : defaultValue;
    #endregion

    #region " --> AsSingle                                "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Single zurück,
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="invariant">gibt an, ob ein Sprach-/ Kulturunabhängiges Format verwendet wird</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static Single? AsSingle(this String value, Boolean invariant = true, Single? defaultValue = default) => String.IsNullOrEmpty(value)
        ? defaultValue
        : !invariant && Single.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out Single result)
        ? new Single?(result)
        : Single.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result) ? new Single?(result) : defaultValue;
    #endregion

    #region " --> AsDouble                                "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Double zurück
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="invariant">gibt an, ob ein Sprach-/ Kulturunabhängiges Format verwendet wird</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static Double? AsDouble(this String value, Boolean invariant = true, Double? defaultValue = default) => String.IsNullOrEmpty(value)
        ? defaultValue
        : !invariant && Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out Double result)
        ? new Double?(result)
        : Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result) ? new Double?(result) : defaultValue;
    #endregion

    #region " --> AsBoolean                               "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable Boolean zurück,
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static Boolean? AsBoolean(this String value, Boolean? defaultValue = default) => String.IsNullOrEmpty(value) ? defaultValue : Boolean.TryParse(value, out Boolean result) ? new Boolean?(result) : defaultValue;
    #endregion

    #region " --> AsDateTime                              "
    /// <summary>
    /// Liefert die Zeichenkette als Nullable DateTime zurück,
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="invariant">gibt an, ob ein Sprach-/ Kulturunabhängiges Format verwendet wird</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static DateTime? AsDateTime(this String value, Boolean invariant = true, DateTime? defaultValue = default) => String.IsNullOrEmpty(value)
        ? defaultValue
        : !invariant && DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result)
        ? new DateTime?(result)
        : DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ? new DateTime?(result) : defaultValue;
    #endregion

    #region " --> AsTimeSpan                              "
    /// <summary>
    /// Liefert die Zeichenkette als TimeSpan zurück,
    /// falls nicht castbar wird der Standartwert aus dem Parameter
    /// <paramref name="defaultValue"/> zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <param name="defaultValue">Standard-Rückgabe wenn nicht castbar</param>
    /// <returns>umgewandelter Wert oder Standartwert</returns>
    public static TimeSpan? AsTimeSpan(this String value, TimeSpan? defaultValue = default) => String.IsNullOrEmpty(value)
        ? defaultValue
        : TimeSpan.TryParse(value, out TimeSpan result) ? new TimeSpan?(result) : defaultValue;
    #endregion

    #region " --> AsXml                                   "
    /// <summary>
    /// Liefert die Zeichenkette als XML-Dokument
    /// zurück, wenn nicht möglich wird NULL
    /// zurückgegeben
    /// </summary>
    /// <param name="value">zu konvertierende Zeichenfolge</param>
    /// <returns>XML-DOkument oder Null</returns>
    public static IXPathNavigable AsXml(this String value)
    {
      if (String.IsNullOrEmpty(value)) { return default; }

      try
      {
        XmlDocument xmlDocument = new();
        xmlDocument.LoadXml(value);
        return xmlDocument;
      }
      catch (XmlException)
      {
        return default;
      }
    }
    #endregion

    #endregion
  }
}

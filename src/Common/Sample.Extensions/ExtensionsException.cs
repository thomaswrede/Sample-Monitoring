using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;

namespace System
{
  /// <summary>
  /// Stellt Erweiterungsmethoden für <see cref="Exception"/> zur Verfügung,
  /// um detaillierte Informationen und Daten aus Ausnahmen zu extrahieren.
  /// </summary>
  public static class ExtensionsException
  {
    #region " öffentliche Methoden                        "

    #region " --> ToDetailedString                        "
    /// <summary>
    /// Gibt eine detaillierte Zeichenfolgenrepräsentation der angegebenen <see cref="Exception"/> und aller InnerExceptions zurück.
    /// </summary>
    /// <param name="exception">Die zu verarbeitende <see cref="Exception"/>.</param>
    /// <returns>
    /// Eine Zeichenfolge, welche die Exception und alle InnerExceptions enthält.
    /// </returns>
    public static String ToDetailedString(this Exception exception)
    {
      StringBuilder sb = new();
      while (exception != null)
      {
        _ = sb.AppendLine(exception.ToString());
        exception = exception.InnerException;
      }
      return sb.ToString();
    }
    #endregion

    #region " --> GetExceptionData                        "
    /// <summary>
    /// Extrahiert die Daten aus der angegebenen <see cref="Exception"/> und fügt sie dem bereitgestellten Dictionary hinzu.
    /// Bei <see cref="SqlException"/> werden zusätzliche SQL-spezifische Informationen ergänzt.
    /// </summary>
    /// <param name="exception">Die zu verarbeitende <see cref="Exception"/>.</param>
    /// <param name="data">
    /// Das Dictionary, dem die Exception-Daten hinzugefügt werden.
    /// Falls <c>null</c>, wird ein neues Dictionary erstellt.
    /// </param>
    /// <returns>
    /// Eine Zeichenfolge, welche die gesammelten Exception-Daten im Format <c>Key:Value#Key:Value...</c> enthält.
    /// </returns>
    public static String GetExceptionData(this Exception exception, IDictionary<String, ReadOnlyMemory<Char>> data)
    {
      data ??= new Dictionary<String, ReadOnlyMemory<Char>>();
      if (exception != null)
      {
        foreach (String key in exception.Data.Keys)
        {
          data.Add(key, exception.Data[key].ToString().AsMemory());
        }

        if (exception is SqlException sqlException)
        {
          data.Add("Number", sqlException.Number.ToString().AsMemory());
          data.Add("Class", sqlException.Class.ToString().AsMemory());
          data.Add("State", sqlException.State.ToString().AsMemory());
          data.Add("Server", sqlException.Server.AsMemory());
          data.Add("Procedure", sqlException.Procedure.AsMemory());
          data.Add("ClientConnectionId", sqlException.ClientConnectionId.AsMemory());
          data.Add("IsTransient", sqlException.IsTransient.ToString().AsMemory());
          data.Add("SqlState", sqlException.SqlState?.AsMemory() ?? ReadOnlyMemory<Char>.Empty);
          data.Add("LineNumber", sqlException.LineNumber.ToString().AsMemory());
        }
      }
      return data.AggregateToString("#", ":");
    }
    #endregion

    #endregion
  }
}

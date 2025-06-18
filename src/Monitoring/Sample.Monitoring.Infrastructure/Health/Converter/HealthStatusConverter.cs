using System;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion
{
  /// <summary>
  /// Konvertiert <see cref="HealthStatus"/>-Werte in <see cref="Int32"/> und umgekehrt für die Speicherung in einer Datenbank.
  /// </summary>
  public sealed class HealthStatusConverter : ValueConverter<HealthStatus, Int32>
  {
    #region " Konstruktor                                 "

    #region " --> New                                     "
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="HealthStatusConverter"/>-Klasse.
    /// Definiert die Konvertierungslogik zwischen <see cref="HealthStatus"/> und <see cref="Int32"/>.
    /// </summary>
    public HealthStatusConverter()
       : base(
           v => (Int32)v,
           v => (HealthStatus)v)
    { }
    #endregion

    #endregion
  }
}

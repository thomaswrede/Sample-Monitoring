using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health.System
{
  /// <summary>
  /// Stellt Erweiterungsmethoden für <see cref="IHealthChecksBuilder"/> zur Verfügung,
  /// um System-HealthChecks wie die Überprüfung des freien Speicherplatzes auf Datenträgern hinzuzufügen.
  /// </summary>
  public static class ExtensionsSystemHealthCheckBuilder
  {
    #region " öffentliche Methoden                        "

    #region " --> AddDiskStorageCheck                     "
    /// <summary>
    /// Fügt dem <see cref="IHealthChecksBuilder"/> einen HealthCheck zur Überwachung des freien Speicherplatzes auf Datenträgern hinzu.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, dem der HealthCheck hinzugefügt werden soll.</param>
    /// <param name="driveNamesBuilder">
    /// Eine Funktion, die eine Auflistung von Tupeln mit Laufwerksnamen und minimal erforderlichem freien Speicherplatz (in Byte) zurückgibt.
    /// </param>
    /// <param name="name">Der Name des HealthChecks.</param>
    /// <param name="failureStatus">Der Status, der bei einem Fehler zurückgegeben werden soll (optional).</param>
    /// <param name="tags">Eine optionale Liste von Tags für den HealthCheck.</param>
    /// <param name="timeout">Ein optionales Timeout für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddDiskStorageCheck(this IHealthChecksBuilder builder,
            Func<IEnumerable<(String DriveName, Int64 MinimumFreeSpace)>> driveNamesBuilder,
            String name,
            HealthStatus? failureStatus = default,
            IEnumerable<String> tags = default,
            TimeSpan? timeout = default) => builder.Add(new HealthCheckRegistration(
          name,
          new HealthCheckDiskStorage(driveNamesBuilder()),
          failureStatus,
          tags,
          timeout));

    /// <summary>
    /// Fügt dem <see cref="IHealthChecksBuilder"/> einen HealthCheck zur Überwachung des freien Speicherplatzes auf Datenträgern hinzu,
    /// wobei die Konfiguration über ein <see cref="HealthCheckDiskStorageOptions"/>-Objekt erfolgt.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, dem der HealthCheck hinzugefügt werden soll.</param>
    /// <param name="options">Die Optionen für die Konfiguration des HealthChecks.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddDiskStorageCheck(this IHealthChecksBuilder builder,
            HealthCheckDiskStorageOptions options) => builder.Add(new HealthCheckRegistration(
          options.Name,
          new HealthCheckDiskStorage(options.Drives),
          default,
          options.Tags,
          default));
    #endregion

    #endregion

  }
}

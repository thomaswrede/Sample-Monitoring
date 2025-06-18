using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace Sample.Monitoring.Health.Application
{
  /// <summary>
  /// Erweiterungsmethoden für <see cref="IHealthChecksBuilder"/> zur Registrierung von anwendungsspezifischen HealthChecks.
  /// </summary>
  public static class ExtensionsApplicationHealthCheckBuilder
  {
    #region " öffentliche Methoden                        "

    #region " --> AddApplicationStatusCheck               "
    /// <summary>
    /// Fügt einen HealthCheck hinzu, der den Status der Anwendung überwacht.
    /// Gibt <see cref="HealthStatus.Healthy"/> zurück, solange die Anwendung nicht gestoppt wird.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, an den der HealthCheck angehängt wird.</param>
    /// <param name="applicationLifetimeBuilder">Funktion, die eine Instanz von <see cref="IHostApplicationLifetime"/> bereitstellt.</param>
    /// <param name="name">Der Name des HealthChecks.</param>
    /// <param name="failureStatus">Der Status, der zurückgegeben wird, wenn der HealthCheck fehlschlägt (optional).</param>
    /// <param name="tags">Tags zur Kategorisierung des HealthChecks (optional).</param>
    /// <param name="timeout">Maximale Ausführungsdauer des HealthChecks (optional).</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem registrierten HealthCheck.</returns>
    public static IHealthChecksBuilder AddApplicationStatusCheck(
      this IHealthChecksBuilder builder,
      Func<IHostApplicationLifetime> applicationLifetimeBuilder,
      String name,
      HealthStatus? failureStatus = default,
      IEnumerable<String> tags = default,
      TimeSpan? timeout = default) => builder.Add(
          new HealthCheckRegistration(
              name,
              _ => new HealthCheckApplicationStatus(applicationLifetimeBuilder()),
              failureStatus,
              tags,
              timeout));
    #endregion

    #region " --> AddAllocatedMemoryCheck                 "
    /// <summary>
    /// Fügt einen HealthCheck hinzu, der den aktuell allokierten Speicher überwacht.
    /// Gibt einen Fehlerstatus zurück, wenn der zugewiesene Speicher einen definierten Grenzwert überschreitet.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, an den der HealthCheck angehängt wird.</param>
    /// <param name="maximumMemoryBuilder">Funktion, die den maximal erlaubten Speicher (in Byte) liefert.</param>
    /// <param name="name">Der Name des HealthChecks.</param>
    /// <param name="failureStatus">Der Status, der zurückgegeben wird, wenn der HealthCheck fehlschlägt (optional).</param>
    /// <param name="tags">Tags zur Kategorisierung des HealthChecks (optional).</param>
    /// <param name="timeout">Maximale Ausführungsdauer des HealthChecks (optional).</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem registrierten HealthCheck.</returns>
    public static IHealthChecksBuilder AddAllocatedMemoryCheck(
      this IHealthChecksBuilder builder,
      Func<Int64> maximumMemoryBuilder,
      String name,
      HealthStatus? failureStatus = default,
      IEnumerable<String> tags = default,
      TimeSpan? timeout = default) => builder.Add(
          new HealthCheckRegistration(
              name,
              _ => new HealthCheckAllocatedMemory(maximumMemoryBuilder()),
              failureStatus,
              tags,
              timeout));
    /// <summary>
    /// Fügt einen HealthCheck hinzu, der den aktuell allokierten Speicher überwacht, mit Optionen aus <see cref="HealthCheckAllocatedMemoryOptions"/>.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, an den der HealthCheck angehängt wird.</param>
    /// <param name="options">Die Konfigurationsoptionen für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem registrierten HealthCheck.</returns>
    public static IHealthChecksBuilder AddAllocatedMemoryCheck(
      this IHealthChecksBuilder builder,
      HealthCheckAllocatedMemoryOptions options) => builder.Add(
          new HealthCheckRegistration(
              options.Name,
              _ => new HealthCheckAllocatedMemory(options.MaximumMemory),
              default,
              options.Tags,
              default));
    #endregion

    #endregion

  }
}

using System;
using System.Collections.Generic;
using System.Net;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Stellt Erweiterungsmethoden für <see cref="IHealthChecksBuilder"/> bereit, um verschiedene Netzwerk-HealthChecks hinzuzufügen.
  /// </summary>
  public static class ExtensionsNetworkHealthCheckBuilder
  {
    #region " öffentliche Methoden                        "

    #region " --> AddSignalRHubCheck                      "
    /// <summary>
    /// Fügt einen HealthCheck für eine SignalR-Hub-Verbindung hinzu.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, zu dem der HealthCheck hinzugefügt wird.</param>
    /// <param name="hubConnectionBuilder">Eine Funktion, die eine neue <see cref="HubConnection"/> erstellt.</param>
    /// <param name="name">Der Name des HealthChecks.</param>
    /// <param name="failureStatus">Der Status, der bei einem Fehler zurückgegeben wird (optional).</param>
    /// <param name="tags">Optionale Tags für den HealthCheck.</param>
    /// <param name="timeout">Optionales Timeout für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddSignalRHubCheck(
      this IHealthChecksBuilder builder,
      Func<HubConnection> hubConnectionBuilder,
      String name,
      HealthStatus? failureStatus = default,
      IEnumerable<String> tags = default,
      TimeSpan? timeout = default) => builder.Add(
          new HealthCheckRegistration(
              name,
              _ => new HealthCheckSignalR(hubConnectionBuilder),
              failureStatus,
              tags,
              timeout));
    #endregion

    #region " --> AddSmtpServerCheck                      "
    /// <summary>
    /// Fügt einen HealthCheck für einen SMTP-Server hinzu.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, zu dem der HealthCheck hinzugefügt wird.</param>
    /// <param name="smtpSettingsBuilder">Eine Funktion, welche die <see cref="SmtpSettings"/> für den HealthCheck bereitstellt.</param>
    /// <param name="name">Der Name des HealthChecks.</param>
    /// <param name="failureStatus">Der Status, der bei einem Fehler zurückgegeben wird (optional).</param>
    /// <param name="tags">Optionale Tags für den HealthCheck.</param>
    /// <param name="timeout">Optionales Timeout für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddSmtpServerCheck(
      this IHealthChecksBuilder builder,
      Func<SmtpSettings> smtpSettingsBuilder,
      String name,
      HealthStatus? failureStatus = default,
      IEnumerable<String> tags = default,
      TimeSpan? timeout = default) => builder.Add(
          new HealthCheckRegistration(
              name,
              _ => new HealthCheckSmtpServer(smtpSettingsBuilder()),
              failureStatus,
              tags,
              timeout));

    /// <summary>
    /// Fügt einen HealthCheck für einen SMTP-Server mit vordefinierten Optionen hinzu.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, zu dem der HealthCheck hinzugefügt wird.</param>
    /// <param name="options">Die <see cref="HealthCheckSmtpServerOptions"/> für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddSmtpServerCheck(
      this IHealthChecksBuilder builder,
      HealthCheckSmtpServerOptions options) => builder.Add(
          new HealthCheckRegistration(
              options.Name,
              _ => new HealthCheckSmtpServer(options.SmtpSettings),
              default,
              options.Tags,
              default));
    #endregion

    #region " --> AddDnsCheck                             "
    /// <summary>
    /// Fügt einen HealthCheck für die DNS-Auflösung einer Liste von Hostnamen hinzu.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, zu dem der HealthCheck hinzugefügt wird.</param>
    /// <param name="hostNamesBuilder">Eine Funktion, welche die zu prüfenden Hostnamen bereitstellt.</param>
    /// <param name="name">Der Name des HealthChecks.</param>
    /// <param name="failureStatus">Der Status, der bei einem Fehler zurückgegeben wird (optional).</param>
    /// <param name="tags">Optionale Tags für den HealthCheck.</param>
    /// <param name="timeout">Optionales Timeout für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddDnsCheck(
      this IHealthChecksBuilder builder,
      Func<IEnumerable<String>> hostNamesBuilder,
      String name,
      HealthStatus? failureStatus = default,
      IEnumerable<String> tags = default,
      TimeSpan? timeout = default) => builder.Add(
          new HealthCheckRegistration(
              name,
              _ => new HealthCheckDns(hostNamesBuilder()),
              failureStatus,
              tags,
              timeout));

    /// <summary>
    /// Fügt einen HealthCheck für die DNS-Auflösung mit vordefinierten Optionen hinzu.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, zu dem der HealthCheck hinzugefügt wird.</param>
    /// <param name="options">Die <see cref="HealthCheckDnsOptions"/> für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddDnsCheck(
      this IHealthChecksBuilder builder,
      HealthCheckDnsOptions options) => builder.Add(
          new HealthCheckRegistration(
              options.Name,
              new HealthCheckDns(options.HostNames),
              default,
              options.Tags,
              default));
    #endregion

    #region " --> AddFtpCheck                             "
    /// <summary>
    /// Fügt einen HealthCheck für FTP-Server hinzu.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, zu dem der HealthCheck hinzugefügt wird.</param>
    /// <param name="setup">Eine Aktion zur Konfiguration der <see cref="HealthCheckFtpOptions"/>.</param>
    /// <param name="name">Der Name des HealthChecks.</param>
    /// <param name="failureStatus">Der Status, der bei einem Fehler zurückgegeben wird (optional).</param>
    /// <param name="tags">Optionale Tags für den HealthCheck.</param>
    /// <param name="timeout">Optionales Timeout für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddFtpCheck(
            this IHealthChecksBuilder builder,
            Action<HealthCheckFtpOptions> setup,
            String name,
            HealthStatus? failureStatus = default,
            IEnumerable<String> tags = default,
            TimeSpan? timeout = default)
    {
      HealthCheckFtpOptions options = new();
      setup?.Invoke(options);

      return builder.Add(new HealthCheckRegistration(
          name,
          _ => new HealthCheckFtp(options),
          failureStatus,
          tags,
          timeout));
    }
    #endregion

    #region " --> AddRestCheck                            "
    /// <summary>
    /// Fügt einen REST-basierten HealthCheck hinzu.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, zu dem der HealthCheck hinzugefügt wird.</param>
    /// <param name="setup">Eine Aktion zur Konfiguration der <see cref="HealthCheckRestOptions"/>.</param>
    /// <param name="name">Der Name des HealthChecks.</param>
    /// <param name="failureStatus">Der Status, der bei einem Fehler zurückgegeben wird (optional).</param>
    /// <param name="tags">Optionale Tags für den HealthCheck.</param>
    /// <param name="timeout">Optionales Timeout für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddRestCheck(
            this IHealthChecksBuilder builder,
            Action<HealthCheckRestOptions> setup,
            String name,
            HealthStatus? failureStatus = default,
            IEnumerable<String> tags = default,
            TimeSpan? timeout = default)
    {
      HealthCheckRestOptions options = new();
      setup?.Invoke(options);

      return builder.Add(new HealthCheckRegistration(
          name,
          _ => new HealthCheckRest(options),
          failureStatus,
          tags,
          timeout));
    }

    /// <summary>
    /// Fügt einen REST-basierten HealthCheck mit vordefinierten Optionen hinzu.
    /// </summary>
    /// <param name="builder">Der <see cref="IHealthChecksBuilder"/>, zu dem der HealthCheck hinzugefügt wird.</param>
    /// <param name="options">Die <see cref="HealthCheckRestOptions"/> für den HealthCheck.</param>
    /// <returns>Der <see cref="IHealthChecksBuilder"/> mit dem hinzugefügten HealthCheck.</returns>
    public static IHealthChecksBuilder AddRestCheck(
      this IHealthChecksBuilder builder,
      HealthCheckRestOptions options) => builder.Add(new HealthCheckRegistration(
          options.Name,
          _ => new HealthCheckRest(options),
          default,
          options.Tags,
          default));
    #endregion

    #endregion

  }
}

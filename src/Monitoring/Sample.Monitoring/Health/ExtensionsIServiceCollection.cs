using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Sample.Monitoring.Health.Application;
using Sample.Monitoring.Health.Network;
using Sample.Monitoring.Health.System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Sample.Monitoring.Health
{
  /// <summary>
  /// Stellt Erweiterungsmethoden für <see cref="IServiceCollection"/> und <see cref="IApplicationBuilder"/> zur Konfiguration und Nutzung von HealthChecks bereit.
  /// </summary>
  public static class ExtensionsIServiceCollectionHealthChecks
  {
    #region " öffentliche Methoden                        "

    #region " --> AddHealthCheckMonitor                   "
    /// <summary>
    /// Fügt die HealthCheck-Monitoring-Dienste zur ServiceCollection hinzu und konfiguriert HealthChecks anhand der Konfiguration.
    /// </summary>
    /// <param name="services">Die zu erweiternde <see cref="IServiceCollection"/>.</param>
    /// <param name="setupSettings">Optionale Aktion zur weiteren Konfiguration der <see cref="HealthCheckMonitorSettings"/>.</param>
    /// <returns>Ein <see cref="IHealthChecksBuilder"/> zur weiteren Konfiguration von HealthChecks.</returns>
    public static IHealthChecksBuilder AddHealthCheckMonitor(this IServiceCollection services, Action<HealthCheckMonitorSettings> setupSettings = default)
    {
      OptionsBuilder<HealthCheckMonitorSettings> optionsBuilder = services
        .AddOptions<HealthCheckMonitorSettings>()
        .Configure<IConfiguration>((settings, configuration) =>
        {
          configuration.GetSection("Health")
            .Bind(settings);
          configuration.GetSection("Health:HealthChecks")?.GetChildren().ToArray().ForAll(c =>
          {
            String type = c.GetValue<String>("Type");
            HealthCheckSettings options = default;
            switch (type.ToUpperInvariant())
            {
              //Application
              case "APPLICATION STATE":
                options = new HealthCheckApplicationStatusOptions().Init(c);
                break;
              case "ALLOCATED MEMORY":
                options = new HealthCheckAllocatedMemoryOptions().Init(c);
                break;
              //System
              case "DISK STORAGE":
                options = new HealthCheckDiskStorageOptions().Init(c);
                break;
              //Network
              case "SMTP":
                options = new HealthCheckSmtpServerOptions().Init(c);
                break;
              case "DNS":
                options = new HealthCheckDnsOptions().Init(c);
                break;
              case "REST":
                options = new HealthCheckRestOptions().Init(c);
                break;
                //Database
            }
            if (settings != default)
            {
              _ = settings.AddHealthCheckOptions(options);
            }
          });

          setupSettings?.Invoke(settings);
        });

      IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

      if (configuration != default)
      {
        _ = optionsBuilder.Bind(configuration.GetSection("Health"))
          .Configure(options => options.ServerName ??= Environment.MachineName);
      }

      _ = services.AddHttpClient("Sample.Monitoring.API", client => client.BaseAddress = new Uri(configuration.GetSection("Health:StorageApi").Value));
      _ = services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Sample.Monitoring.API"));

      _ = services.AddSingleton<HealthCheckMonitorService>();
      IHealthChecksBuilder healthChecksBuilder = services.AddHealthChecks();
      //Workaround: da ich bisher nicht herausfinden konnte, wie die Bindung der Konfiguration hier ausgelöst werden kann, oder innerhalb des HealthCheckMonitorService die HealthChecks registriert werden können, muss hier zunächst die Konfiguration übergeben werden und manuell ausgelesen und damit die Checks registriert werden
      configuration.GetSection("Health:HealthChecks")?.GetChildren().ToArray().ForAll(healthCheckSetting =>
      {
        String type = healthCheckSetting.GetValue<String>("Type");
        switch (type.ToUpperInvariant())
        {
          //Application
          //case "APPLICATION STATE":
          //  _ = healthChecksBuilder.AddApplicationStatusCheck(new HealthCheckApplicationStatusOptions().Init(healthCheckSetting) as HealthCheckApplicationStatusOptions);
          //  break;
          case "ALLOCATED MEMORY":
            _ = healthChecksBuilder.AddAllocatedMemoryCheck(new HealthCheckAllocatedMemoryOptions().Init(healthCheckSetting) as HealthCheckAllocatedMemoryOptions);
            break;
          //System
          case "DISK STORAGE":
            _ = healthChecksBuilder.AddDiskStorageCheck(new HealthCheckDiskStorageOptions().Init(healthCheckSetting) as HealthCheckDiskStorageOptions);
            break;
          //Network
          case "SMTP":
            _ = healthChecksBuilder.AddSmtpServerCheck(new HealthCheckSmtpServerOptions().Init(healthCheckSetting) as HealthCheckSmtpServerOptions);
            break;
          case "DNS":
            _ = healthChecksBuilder.AddDnsCheck(new HealthCheckDnsOptions().Init(healthCheckSetting) as HealthCheckDnsOptions);
            break;
          case "REST":
            _ = healthChecksBuilder.AddRestCheck(new HealthCheckRestOptions().Init(healthCheckSetting) as HealthCheckRestOptions);
            break;
            //Database
        }
      });
      return healthChecksBuilder;
    }
    #endregion

    #region " --> UseHealthCheckMonitor                   "
    /// <summary>
    /// Startet den HealthCheck-Monitor-Service beim Anwendungsstart.
    /// </summary>
    /// <param name="builder">Der <see cref="IApplicationBuilder"/>, der erweitert werden soll.</param>
    /// <returns>Der erweiterte <see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseHealthCheckMonitor(this IApplicationBuilder builder)
    {
      builder.ApplicationServices.GetRequiredService<HealthCheckMonitorService>()?.Start();
      return builder;
    }
    #endregion

    #endregion
  }
}

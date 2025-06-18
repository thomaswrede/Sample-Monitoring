using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Sample.Monitoring.Health;

namespace Sample.Monitoring.DependencyInjection
{
  /// <summary>
  /// Stellt Erweiterungsmethoden zur Verfügung, um Monitoring-Funktionalitäten in die Anwendung zu integrieren.
  /// </summary>
  public static class MonitoringExtensions
  {
    #region " öffentliche Methoden                        "

    #region " --> AddMonitoring                           "
    /// <summary>
    /// Registriert die für das Monitoring erforderlichen Dienste in der Dependency Injection.
    /// </summary>
    /// <param name="services">Die <see cref="IServiceCollection"/>, zu der die Monitoring-Dienste hinzugefügt werden.</param>
    /// <returns>Die aktualisierte <see cref="IServiceCollection"/> mit den registrierten Monitoring-Diensten.</returns>
    public static IServiceCollection AddMonitoring(this IServiceCollection services)
    {
      _ = services
        .AddHealthCheckMonitor();
      return services;
    }
    #endregion

    #region " --> UseMonitoring                           "
    /// <summary>
    /// Integriert die Monitoring-Middleware in die Anforderungspipeline der Anwendung.
    /// </summary>
    /// <param name="builder">Der <see cref="IApplicationBuilder"/>, der zum Konfigurieren der Anforderungspipeline verwendet wird.</param>
    /// <returns>Der <see cref="IApplicationBuilder"/> mit integrierter Monitoring-Middleware.</returns>
    public static IApplicationBuilder UseMonitoring(this IApplicationBuilder builder) => builder.UseHealthCheckMonitor();
    #endregion

    #endregion

  }
}

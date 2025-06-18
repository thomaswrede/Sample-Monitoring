using Sample.Monitoring.DependencyInjection;
using Sample.Monitoring.UI.Services.Health;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Monitoring.UI.DependencyInjection
{
  public static class MonitoringUIExtensions
  {
    #region " öffentliche Methoden                        "

    #region " --> AddMonitoringUI                         "
    public static IServiceCollection AddMonitoringClientUI(this IServiceCollection services)
    {
      _ = services
        .AddMonitoring()
        .AddScoped<HealthStatusService>();

      return services;
    }
    #endregion

    #region " --> UseMonitoringUI                           "
    public static IApplicationBuilder UseMonitoringUI(this IApplicationBuilder builder) =>
      builder.UseMonitoring();
    #endregion

    #endregion
  }
}

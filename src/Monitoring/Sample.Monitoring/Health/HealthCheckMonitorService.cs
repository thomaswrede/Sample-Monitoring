using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Sample.Monitoring.Model.Health;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Sample.Monitoring.Health
{
  internal class HealthCheckMonitorService : IHostedService
  {
    #region " Variablen/ Properties                       "

    #region " --> LifeTime                                "
    private readonly IHostApplicationLifetime _LifeTime;
    #endregion

    #region " --> ServiceProvider                         "
    private readonly IServiceProvider _ServiceProvider;
    #endregion

    #region " --> Settings                                "
    private readonly HealthCheckMonitorSettings _Settings;
    #endregion

    #region " --> CancellationTokenSource                 "
    private readonly CancellationTokenSource _CancellationTokenSource;
    #endregion

    #region " --> HealthCheckCollectTask                  "
    private Task _HealthCheckCollectTask;
    #endregion

    #region " --> HttpClient                              "
    private readonly HttpClient _HttpClient;
    #endregion

    #endregion

    #region " Konstruktor                                 "

    #region " --> New                                     "
    public HealthCheckMonitorService
      (IServiceProvider provider,
      IOptions<HealthCheckMonitorSettings> settings,
      IHostApplicationLifetime lifetime,
      IHttpClientFactory httpClientFactory)
    {
      this._ServiceProvider = provider;
      this._LifeTime = lifetime;
      this._Settings = settings?.Value ?? new HealthCheckMonitorSettings();
      this._CancellationTokenSource = new CancellationTokenSource();
      _ = this._LifeTime?.ApplicationStopping.Register(this._CancellationTokenSource.Cancel);
      this._HttpClient = httpClientFactory.CreateClient("Sample.Monitoring.API");
    }
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Start/Async                             "
    public void Start()
    {
      _ = this._Settings.HealthChecks.ForAll(check => _ = this._HttpClient.PostAsJsonAsync<HealthCheck>("api/HealthMonitor/RegisterHealthCheck",
          new HealthCheck()
          {
            Name = check.Name,
            Description = check.Description,
            Tags = check.Tags
          }).GetAwaiterResult()
      );

      this._HealthCheckCollectTask = this.CollectAsync(this._CancellationTokenSource.Token);
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
      _ = await this._Settings.HealthChecks.ForAllAsync(async check => _ = await this._HttpClient.PostAsJsonAsync("api/HealthMonitor/RegisterHealthCheck",
          new HealthCheck()
          {
            Name = check.Name,
            Description = check.Description,
            Tags = check.Tags
          })
      );

      await this.CollectAsync(cancellationToken);
    }
    #endregion

    #region " --> StopAsync                               "
    public async Task StopAsync(CancellationToken cancellationToken)
    {
      this._CancellationTokenSource.Cancel();
      if (this._HealthCheckCollectTask != null)
      {
        _ = await Task.WhenAny(this._HealthCheckCollectTask, Task.Delay(Timeout.Infinite, cancellationToken));
      }
    }
    #endregion

    #endregion

    #region " interne Methoden                            "

    #region " --> CollectAsync                            "
    private async Task CollectAsync(CancellationToken cancellationToken)
    {
      _ = this._LifeTime.ApplicationStarted.Register(async () =>
      {
        try
        {
          while (!cancellationToken.IsCancellationRequested)
          {
            HealthCheckService service = this._ServiceProvider?.GetRequiredService<HealthCheckService>();
            HealthReport report = await service?.CheckHealthAsync(cancellationToken);
            await report?.Entries.ForAllAsync(async (e) =>
              _ = await this._HttpClient.PostAsJsonAsync("api/HealthMonitor/AddHealthCheck",
              new HealthCheckEntry()
              {
                ServerName = this._Settings.ServerName,
                ApplicationId = this._Settings.ApplicationId,
                Name = e.Key,
                Description = e.Value.Description,
                LastDuration = e.Value.Duration,
                LastExecution = DateTime.Now,
                Status = e.Value.Status
              }),
              (_, ex) => Log.Error(default, exception: ex)
            );
            await Task.Delay(this._Settings.CheckInterval * 1000, cancellationToken);
          }
        }
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested) { }
      });

      await Task.CompletedTask;
    }
    #endregion

    #endregion

  }
}

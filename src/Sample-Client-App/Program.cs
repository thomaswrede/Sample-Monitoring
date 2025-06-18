using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

using Sample.Logging.UI.DependencyInjection;
using Sample.Monitoring.UI.DependencyInjection;

using Sample_Client_App.Components;

namespace Sample_Client_App
{
  public static class Program
  {
    public static void Main(String[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddRazorComponents()
          .AddInteractiveServerComponents();
      builder.Services.AddFluentUIComponents();

      _ = builder.Services
        .AddAdvancedLogging()
        .AddMonitoringClientUI();

      WebApplication app = builder.Build();

      // Configure the HTTP request pipeline.
      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app
        .UseAdvancedLogging(default)
        .UseHttpsRedirection()
        .UseStaticFiles()
        .UseRouting()
        .UseMonitoringUI();
      app.UseAntiforgery();

      app.MapRazorComponents<App>()
        .AddAdditionalAssemblies([typeof(Sample.Monitoring.UI.Areas.Health.Components.ApplicationState).Assembly])
        .AddInteractiveServerRenderMode();

      app.Run();
    }
  }
}

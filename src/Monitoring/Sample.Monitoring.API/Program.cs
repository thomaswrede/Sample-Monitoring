
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Sample.Logging.UI.DependencyInjection;
using Sample.Monitoring.API.DependencyInjection;

namespace Sample.Monitoring.API
{
  /// <summary>
  /// Stellt den Einstiegspunkt f�r die Sample.Monitoring.API-Anwendung dar.
  /// </summary>
  public static class Program
  {
    /// <summary>
    /// Hauptmethode der Anwendung. Initialisiert und startet die Webanwendung.
    /// </summary>
    /// <param name="args">Kommandozeilenargumente f�r die Anwendung.</param>
    private static async Task Main(String[] args)
    {
      // Registriert einen Handler f�r FirstChanceException, um Fehler zu protokollieren.
      AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) => Log.Error(default, eventArgs.Exception, $"FirstChanceException: {eventArgs.Exception.Message}".AsMemory());

      // Erstellt den WebApplicationBuilder mit den �bergebenen Argumenten.
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      // F�gt erweiterte Logging- und Monitoring-Storage-Dienste hinzu.
      _ = builder.Services
        .AddAdvancedLogging()
        .AddMonitoringStorage();

      // F�gt Controller-Unterst�tzung hinzu.
      _ = builder.Services
        .AddControllers();

      // F�gt Swagger/OpenAPI-Unterst�tzung hinzu.
      _ = builder.Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen();

      // Baut die Webanwendung.
      WebApplication app = builder.Build();

      // Aktiviert das erweiterte Logging mit einem neuen Kontext.
      app.UseAdvancedLogging(new Context(Guid.NewGuid().ToString(), default));

      // Aktiviert Swagger nur in der Entwicklungsumgebung.
      if (app.Environment.IsDevelopment())
      {
        _ = app.UseSwagger();
        _ = app.UseSwaggerUI();
      }

      // Aktiviert Monitoring-Storage.
      app.UseMonitoringStorage();

      // Aktiviert HTTPS-Umleitung und Routing.
      _ = app
        .UseHttpsRedirection()
        .UseRouting();
      //.UseAuthorization(); //Als Programmbeispiel, hier keine Autorisierung

      // Mappt die Controller-Endpunkte.
      _ = app.MapControllers();

      // Startet die Anwendung asynchron.
      await app.RunAsync();
    }
  }
}

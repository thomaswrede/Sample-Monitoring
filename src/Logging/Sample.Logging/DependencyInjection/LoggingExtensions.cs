using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Logging.UI.DependencyInjection
{
  /// <summary>
  /// Stellt Erweiterungsmethoden für die erweiterte Protokollierung (Logging) bereit.
  /// </summary>
  public static class LoggingExtensions
  {
    #region " Konstanten                                  "

    #region " --> EX_SOURCE_CSHARP                        "
    /// <summary>
    /// Quellname für Microsoft.CSharp-Ausnahmen.
    /// </summary>
    private const String EX_SOURCE_CSHARP = "Microsoft.CSharp";
    #endregion 

    #region " --> EX_SOURCE_SYSTEM                        "
    /// <summary>
    /// Quellname für System-Ausnahmen.
    /// </summary>
    private const String EX_SOURCE_SYSTEM = "System";
    #endregion

    #region " --> EX_SOURCE_SOCKETS                       "
    /// <summary>
    /// Quellname für System.Net.Sockets-Ausnahmen.
    /// </summary>
    private const String EX_SOURCE_SOCKETS = "System.Net.Sockets";
    #endregion

    #region " --> EX_SOURCE_NET_SECURITY                  "
    /// <summary>
    /// Quellname für System.Net.Security-Ausnahmen.
    /// </summary>
    private const String EX_SOURCE_NET_SECURITY = "System.Net.Security";
    #endregion

    #region " --> EX_SOURCE_WEB_SOCKETS                   "
    /// <summary>
    /// Quellname für System.Net.WebSockets-Ausnahmen.
    /// </summary>
    private const String EX_SOURCE_WEB_SOCKETS = "System.Net.WebSockets";
    #endregion

    #region " --> EX_SOURCE_ASP_NET_CORE_IIS              "
    /// <summary>
    /// Quellname für Microsoft.AspNetCore.Server.IIS-Ausnahmen.
    /// </summary>
    private const String EX_SOURCE_ASP_NET_CORE_IIS = "Microsoft.AspNetCore.Server.IIS";
    #endregion

    #region " --> EX_TARGET_SYMBOLS                       "
    /// <summary>
    /// Zielmethodenname für Symbol-Ladevorgänge.
    /// </summary>
    private const String EX_TARGET_SYMBOLS = "LoadSymbolsFromType";
    #endregion 

    #region " --> EX_TARGET_BEGIN_WRITE                   "
    /// <summary>
    /// Zielmethodenname für unsichere Schreibvorgänge.
    /// </summary>
    private const String EX_TARGET_BEGIN_WRITE = "UnsafeBeginWrite";
    #endregion

    #region " --> EX_TARGET_END_CONNECT                   "
    /// <summary>
    /// Zielmethodenname für interne Verbindungsabschlüsse.
    /// </summary>
    private const String EX_TARGET_END_CONNECT = "InternalEndConnect";
    #endregion

    #region " --> EX_MESSAGE_RUNTIME_BINDER               "
    /// <summary>
    /// Fehlermeldung für Laufzeitbindungsausnahmen.
    /// </summary>
    private const String EX_MESSAGE_RUNTIME_BINDER = "Microsoft.CSharp.RuntimeBinder.RuntimeBinderException:Die Laufzeitbindung kann für einen NULL-Verweis nicht ausgeführt werden.";
    #endregion

    #region " --> EX_MESSAGE_EA_THREAD_END                "
    /// <summary>
    /// Fehlermeldung für abgebrochene E/A-Vorgänge.
    /// </summary>
    private const String EX_MESSAGE_EA_THREAD_END = "Der E/A-Vorgang wurde wegen eines Threadendes oder einer Anwendungsanforderung abgebrochen";
    #endregion

    #region " --> EX_FILENAME_RESOURCES                   "
    /// <summary>
    /// Dateiname für Ressourcen-DLLs.
    /// </summary>
    private const String EX_FILENAME_RESOURCES = ".resources.dll";
    #endregion

    #region " --> EX_FILENAME_XMLSERIALIZERS              "
    /// <summary>
    /// Dateiname für XML-Serialisierer.
    /// </summary>
    private const String EX_FILENAME_XMLSERIALIZERS = "XmlSerializers";
    #endregion

    #endregion

    #region " Variablen/ Properties                       "

    #region " --> DefaultContext                          "
    /// <summary>
    /// Der Standardkontext für das Logging.
    /// </summary>
    private static IContext _DefaultContext;
    #endregion

    #endregion

    #region " EventHandler                                "

    #region " --> CurrentDomain_UnhandledException        "
    /// <summary>
    /// Behandelt nicht abgefangene Ausnahmen auf AppDomain-Ebene.
    /// </summary>
    /// <param name="sender">Der Sender des Ereignisses.</param>
    /// <param name="e">Ereignisdaten zur Ausnahme.</param>
    private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
    {
      Exception ex = e.ExceptionObject as Exception;
      if (e.IsTerminating || ex != default)
      {
        Log.Error(_DefaultContext, exception: ex, message: Properties.ErrorMessages.UNHANDLED_EXCEPTION_MESSAGE.AsMemory());
      }
    }
    #endregion

    #region " --> CurrentDomain_FirstChanceException      "
    /// <summary>
    /// Behandelt FirstChanceExceptions auf AppDomain-Ebene und filtert bekannte, nicht relevante Ausnahmen heraus.
    /// </summary>
    /// <param name="sender">Der Sender des Ereignisses.</param>
    /// <param name="e">Ereignisdaten zur Ausnahme.</param>
    private static void CurrentDomain_FirstChanceException(Object sender, FirstChanceExceptionEventArgs e)
    {
      Exception ex = e.Exception;
      if (ex != default)
      {
        if (ex is OperationCanceledException)
        {
          return;
        }

        if (ex.Source == EX_SOURCE_CSHARP
          && ex.TargetSite?.Name == EX_TARGET_SYMBOLS)
        {
          //handelt sich um eine Microsoft.CSharp.RuntimeBinder.ResetBindException (internel, daher hier nicht abfragbar)
          //durch DLR verursachte Exception, da immer erst nach statisch definierten Properties gesucht wird. Kein Fehler, sondern durch DLR so implementiert.
          return;
        }

        if (ex.Source is EX_SOURCE_WEB_SOCKETS
          or EX_SOURCE_NET_SECURITY)
        {
          return;
        }

        if (ex is Microsoft.AspNetCore.Components.NavigationException)
        {
          // During static rendering, NavigateTo throws a NavigationException which is handled by the framework as a redirect.
          // So as long as this is called from a statically rendered Identity component, the InvalidOperationException is never thrown.
          return;
        }

        if (ex is RuntimeBinderException bEx != default
          && (bEx.Source == EX_SOURCE_CSHARP
          || bEx.Message == EX_MESSAGE_RUNTIME_BINDER))
        {
          return; //durch DLR verursachte Exception, da immer erst nach statisch definierten Properties gesucht wird. Kein Fehler, sondern durch DLR so implementiert.
        }

        if (ex is ObjectDisposedException dEx
          && ((dEx.Source == EX_SOURCE_SYSTEM
          && dEx.TargetSite?.Name == EX_TARGET_BEGIN_WRITE)
          || dEx.Source == EX_SOURCE_SOCKETS))
        {
          return; //durch SignalR verursachte Exception bei Beendigung von Verbindungen
        }

        if (ex is System.Net.WebSockets.WebSocketException wsEx
          && wsEx?.Source == EX_SOURCE_WEB_SOCKETS)
        {
          return; //durch SignalR verursachte Exception bei Beendigung von Verbindungen
        }

        if (ex is System.IO.IOException ioEx
          && ioEx?.Source == EX_SOURCE_SOCKETS)
        {
          return; //durch SignalR verursachte Exception bei Beendigung von Verbindungen
        }

        if (ex is Microsoft.AspNetCore.Connections.ConnectionResetException crEx
          && crEx?.Source == EX_SOURCE_ASP_NET_CORE_IIS)
        {
          return; //durch SignalR verursachte Exception bei Beendigung von Verbindungen
        }

        if (ex is System.Net.Sockets.SocketException sEx
          && sEx?.Source == EX_SOURCE_SYSTEM
          && sEx.TargetSite?.Name == EX_TARGET_END_CONNECT)
        {
          return; //durch SignalR verursachte Exception bei Beendigung von Verbindungen
        }

        if (ex is Microsoft.JSInterop.JSDisconnectedException)
        {
          return; //durch SignalR verursachte Exception bei Beendigung von Verbindungen
        }

        if (ex is WebException)
        {
          return; //durch Aufrufer behandelt, Auslöser nicht beeinflussbar, da Remote-System
        }

        if (ex is HttpListenerException hEx
          && hEx?.Message == EX_MESSAGE_EA_THREAD_END)
        {
          return; //durch SignalR verursachte Exception bei Beendigung von Verbindungen
        }

        if (ex is System.IO.FileNotFoundException fEx
          && (String.IsNullOrEmpty(fEx.FileName)
          || fEx.FileName.EndsWith(EX_FILENAME_RESOURCES)
          || fEx.FileName.Contains(EX_FILENAME_XMLSERIALIZERS)))
        {
          return; //durch Reflection verursachte Exception, die trotz Einstellung des Verzichts auf XML-Serialisierungsassemblies ausgelöst wird.
        }

        if (ex is Microsoft.Data.SqlClient.SqlException sqlEx
          && (sqlEx.Message.Equals("Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.")
            || sqlEx.Message.StartsWith("In der Datenbank ist bereits ein Objekt mit dem Namen")))
        {
          return; //durch SqlDependency-bedingte Fehlermeldungen, per Design so konzipiert - tatsächliche Timeouts werden per Inceptor geloggt
        }

        Log.Error(_DefaultContext, exception: ex, message: Properties.ErrorMessages.FIRST_CHANCE_EXCEPTION_MESSAGE.AsMemory());
      }
    }
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> AddAdvancedLogging                      "
    /// <summary>
    /// Fügt die erweiterte Logging-Infrastruktur zu den DI-Services hinzu.
    /// </summary>
    /// <param name="services">Die ServiceCollection, zu der Logging hinzugefügt werden soll.</param>
    /// <returns>Die aktualisierte ServiceCollection.</returns>
    public static IServiceCollection AddAdvancedLogging(this IServiceCollection services) =>
      //IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

      //return services
      //  .AddPooledDbContextFactory<LoggingDbContext>(options => options
      //    .UseSqlite(configuration.GetConnectionString("LoggingDb"),
      //      x => x.MigrationsHistoryTable("__MigrationHistory", "Logging"))
      //    .ReplaceService<IMigrationsSqlGenerator, ExtendedMigrationsSqlGenerator>()
      //    .UseCustomSqlServerQuerySqlGenerator()
      //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))
      //  .AddScoped<LoggingDbContextFactory>()
      //  .AddTransient(sp => sp.GetRequiredService<LoggingDbContextFactory>().CreateDbContext());
      services;
    #endregion

    #region " --> UseAdvancedLogging                      "
    /// <summary>
    /// Initialisiert die erweiterte Logging-Infrastruktur und registriert globale Exception-Handler.
    /// </summary>
    /// <param name="builder">Der ApplicationBuilder der Anwendung.</param>
    /// <param name="defaultContext">Der Standardkontext für das Logging.</param>
    /// <returns>Der aktualisierte ApplicationBuilder.</returns>
    public static IApplicationBuilder UseAdvancedLogging(this IApplicationBuilder builder, IContext defaultContext)
    {
      _DefaultContext = defaultContext;
      _ = builder.UseStaticServiceResolver();

      //Hier würde die Action für das Schreiben der Log-Einträge in die Datenbank gesetzt werden.
      //Aktuell wird das Logging zur Vereinfachung des Beispiel-Codes nur in den Speicher geschrieben und nicht persistiert.
      Log.Handle = async (_) => await Task.CompletedTask;
      //using (IServiceScope scope = builder.ApplicationServices.CreateScope())
      //{
      //  LoggingDbContext dbContext = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();
      //  if (dbContext?.Database?.GetPendingMigrations().Any() ?? false)
      //  {
      //    dbContext.Database.Migrate();
      //  }
      //}

      //Log.Handle = async (logEntries) =>
      //{
      //  LoggingDbContext dbContext = StaticServiceResolver.Resolve<LoggingDbContextFactory>(true)?.CreateDbContext();
      //  await dbContext.SaveLogEntriesAsync(logEntries);
      //};

      AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

      return builder;
    }
    #endregion

    #endregion
  }
}

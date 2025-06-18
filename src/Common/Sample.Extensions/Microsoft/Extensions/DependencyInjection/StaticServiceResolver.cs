using System;

using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
  /// <summary>
  /// Stellt statische Methoden zum Auflösen von Services aus dem DI-Container bereit.
  /// </summary>
  public static class StaticServiceResolver
  {
    #region " Variablen/ Properties                       "

    #region " --> ServiceProvider                         "
    /// <summary>
    /// Der statisch gespeicherte <see cref="IServiceProvider"/> der Anwendung.
    /// </summary>
    private static IServiceProvider _ServiceProvider;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> UseStaticServiceResolver                "
    /// <summary>
    /// Initialisiert den statischen ServiceProvider mit den ApplicationServices.
    /// Muss in der Startup-Pipeline aufgerufen werden.
    /// </summary>
    /// <param name="builder">Der <see cref="IApplicationBuilder"/> der Anwendung.</param>
    /// <returns>Der übergebene <see cref="IApplicationBuilder"/> für Method Chaining.</returns>
    public static IApplicationBuilder UseStaticServiceResolver(this IApplicationBuilder builder)
    {
      _ServiceProvider = builder.ApplicationServices;
      return builder;
    }
    #endregion

    #region " --> Resolve                                 "
    /// <summary>
    /// Löst einen Service vom angegebenen Typ aus dem DI-Container auf.
    /// </summary>
    /// <typeparam name="T">Der Typ des aufzulösenden Service.</typeparam>
    /// <param name="scoped">Gibt an, ob der Service in einem neuen Scope aufgelöst werden soll.</param>
    /// <returns>Die Instanz des angeforderten Service.</returns>
    public static T Resolve<T>(Boolean scoped = false) => scoped ? CreateScope().ServiceProvider.GetRequiredService<T>() : _ServiceProvider.GetRequiredService<T>();
    #endregion

    #region " --> CreateScope                             "
    /// <summary>
    /// Erstellt einen neuen <see cref="IServiceScope"/> aus dem statischen ServiceProvider.
    /// </summary>
    /// <returns>Ein neuer <see cref="IServiceScope"/>.</returns>
    public static IServiceScope CreateScope() => _ServiceProvider.CreateScope();
    #endregion

    #region " --> TryResolve                              "
    /// <summary>
    /// Versucht, einen Service vom angegebenen Typ aufzulösen.
    /// </summary>
    /// <typeparam name="T">Der Typ des aufzulösenden Service.</typeparam>
    /// <param name="service">Die aufgelöste Service-Instanz oder der Standardwert, falls nicht gefunden.</param>
    /// <returns><c>true</c>, wenn der Service erfolgreich aufgelöst wurde, andernfalls <c>false</c>.</returns>
    public static Boolean TryResolve<T>(out T service)
    {
      try
      {
        service = _ServiceProvider.GetService<T>();
        return true;
      }
      catch (Exception)
      {
        service = default;
        return false;
      }
    }
    #endregion

    #endregion
  }
}

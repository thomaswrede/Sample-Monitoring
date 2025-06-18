using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Implementiert einen Health-Check für FTP-Server, indem eine Verbindung zu den konfigurierten Hosts aufgebaut wird.
  /// </summary>
  /// <remarks>
  /// Die Klasse überprüft, ob die angegebenen FTP-Hosts erreichbar sind und gibt den Status entsprechend zurück.
  /// </remarks>
  public class HealthCheckFtp(HealthCheckFtpOptions options) : IHealthCheck
  {
    #region " Variablen/ Properties                       "

    #region " --> Options                                 "
    /// <summary>
    /// Die Konfigurationsoptionen für den FTP-Health-Check, einschließlich der zu prüfenden Hosts und deren Zugangsdaten.
    /// </summary>
    private readonly HealthCheckFtpOptions _Options = options;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> CheckHealthAsync                        "
    /// <summary>
    /// Führt den Health-Check für alle konfigurierten FTP-Hosts asynchron aus.
    /// </summary>
    /// <param name="context">Der Kontext für den Health-Check.</param>
    /// <param name="cancellationToken">Ein Token, mit dem der Vorgang abgebrochen werden kann.</param>
    /// <returns>
    /// Ein <see cref="HealthCheckResult"/>, der den Status des Health-Checks für alle FTP-Hosts angibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">Wird ausgelöst, wenn <paramref name="context"/> null ist.</exception>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(context);
      try
      {
        foreach ((String Host, NetworkCredential Credentials) in this._Options.Hosts.Values)
        {
#pragma warning disable SYSLIB0014
          FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(Host);
          ftpRequest.Credentials = Credentials;
          ftpRequest.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;
          using (FtpWebResponse response = (FtpWebResponse)await ftpRequest.GetResponseAsync().WithCancellationTokenAsync(cancellationToken))
          {
            if (response.StatusCode is not FtpStatusCode.PathnameCreated and not FtpStatusCode.ClosingData)
            {
              return new HealthCheckResult(context.Registration.FailureStatus, description: $"Error connecting to ftp host {Host} with exit code {response.StatusCode}");
            }
          }
#pragma warning restore SYSLIB0014
        }
        return HealthCheckResult.Healthy();
      }
      catch (Exception ex)
      {
        return new HealthCheckResult(HealthStatus.Unhealthy, exception: ex);
      }
    }
    #endregion

    #endregion

  }
}

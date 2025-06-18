using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health.Network
{
  /// <summary>
  /// Implementiert einen HealthCheck für einen SMTP-Server, indem eine TCP-Verbindung aufgebaut und die Serverantwort geprüft wird.
  /// </summary>
  public class HealthCheckSmtpServer(SmtpSettings smtpSettings) : IHealthCheck
  {
    #region " Variablen/ Properties                       "

    #region " --> SmtpSettings                            "
    /// <summary>
    /// Die SMTP-Konfigurationseinstellungen für den zu prüfenden Server.
    /// </summary>
    private readonly SmtpSettings _SmtpSettings = smtpSettings;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> CheckHealthAsync                        "
    /// <summary>
    /// Führt die Überprüfung des SMTP-Servers durch, indem eine Verbindung aufgebaut und die Serverantwort ausgewertet wird.
    /// </summary>
    /// <param name="context">Der Kontext für den HealthCheck.</param>
    /// <param name="cancellationToken">Das CancellationToken zur Abbruchsteuerung.</param>
    /// <returns>
    /// Ein <see cref="HealthCheckResult"/>, der den Zustand des SMTP-Servers beschreibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">Wird ausgelöst, wenn <paramref name="context"/> null ist.</exception>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(context);

      SmtpSettings smtpAddress = this._SmtpSettings;
      TcpClient smtpClient;
      try
      {
        using (smtpClient = new())
        {
          await smtpClient.ConnectAsync(smtpAddress.Hostname, smtpAddress.Port ?? (smtpAddress.EnableSsl == true ? 443 : 25), cancellationToken);

          HealthCheckResult healthCheckResult = default;
          if (smtpClient.Connected)
          {
            NetworkStream netStream = smtpClient.GetStream();
            StreamReader sReader = new(netStream);

            //TODO: ggf noch SSL-Zertifikat prüfen?
            healthCheckResult = (await sReader.ReadLineAsync(cancellationToken)).Contains("220")
              ? HealthCheckResult.Healthy()
              : new(context.Registration.FailureStatus, description: "Server-Response not expected");
            smtpClient.Close();
          }
          else
          {
            healthCheckResult = new(context.Registration.FailureStatus, description: "cannot establish connection");
          }

          smtpClient = default;
          return healthCheckResult;
        }
      }
      catch (Exception ex)
      {
        return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
      }
    }
    #endregion

    #endregion

  }
}

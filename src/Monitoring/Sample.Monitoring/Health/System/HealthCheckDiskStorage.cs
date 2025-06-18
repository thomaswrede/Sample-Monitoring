using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health.System
{
  /// <summary>
  /// Implementiert einen HealthCheck zur Überprüfung des freien Speicherplatzes auf angegebenen Laufwerken.
  /// </summary>
  /// <param name="drives">Eine Auflistung von Laufwerken mit jeweils dem Namen und dem minimal erforderlichen freien Speicherplatz (in MByte).</param>
  public class HealthCheckDiskStorage(IEnumerable<(String DriveName, Int64 MinimumFreeSpace)> drives) : IHealthCheck
  {
    #region " Variablen/ Properties                       "

    #region " --> Drives                                  "
    /// <summary>
    /// Liste der zu überprüfenden Laufwerke und deren minimal erforderlicher freier Speicherplatz (in MByte).
    /// </summary>
    private readonly IEnumerable<(String DriveName, Int64 MinimumFreeSpace)> _Drives = drives;

    #endregion
    #endregion

    #region " öffentliche Methoden                        "

    #region " --> CheckHealthAsync                        "
    /// <summary>
    /// Überprüft asynchron, ob auf den konfigurierten Laufwerken ausreichend freier Speicherplatz vorhanden ist.
    /// </summary>
    /// <param name="context">Der HealthCheck-Kontext.</param>
    /// <param name="cancellationToken">Token zum Abbrechen des Vorgangs.</param>
    /// <returns>
    /// Ein <see cref="HealthCheckResult"/>, der den Zustand des HealthChecks beschreibt.
    /// </returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      ArgumentNullException.ThrowIfNull(context);

      try
      {
        List<String> errorMessages = [];
        DriveInfo[] drives = DriveInfo.GetDrives();
        await this._Drives.ForAllAsync(async drive =>
        {
          DriveInfo driveInfo = Array.Find(drives, e => e.Name.Equals(drive.DriveName, StringComparison.OrdinalIgnoreCase));
          if (driveInfo != default)
          {
            Int64 actualFreeMegabytes = driveInfo.AvailableFreeSpace / 1024 / 1024;
            if (actualFreeMegabytes < drive.MinimumFreeSpace)
            {
              errorMessages.Add($"free disk space for {drive.DriveName} was {actualFreeMegabytes} mbytes, minimun free disk space was configured with {drive.MinimumFreeSpace} mbytes");
            }
          }
          else
          {
            errorMessages.Add($"disk {drive.DriveName} not found");
          }

          await Task.CompletedTask;
        });
        return errorMessages.Count != 0
          ? new HealthCheckResult(context.Registration.FailureStatus, description: String.Join("; ", errorMessages))
          : HealthCheckResult.Healthy();
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

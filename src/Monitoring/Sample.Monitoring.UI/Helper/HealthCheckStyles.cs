using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.FluentUI.AspNetCore.Components;

using Sample.Monitoring.Model.Health;

namespace Sample.Monitoring.UI.Helper
{
  internal static class HealthCheckStyles
  {
    #region " öffentliche Methoden                        "

    #region " --> GetStatusColor                          "
    public static Color GetStatusColor(HealthCheckHistoryEntry historyEntry) => historyEntry.Status switch
    {
      Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy => Color.Success,
      Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded => Color.Warning,
      Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy => Color.Error,
      _ => Color.Neutral
    };
    public static Color GetStatusColor(IEnumerable<HealthCheckStateView> checks, String serverName = "")
    {
      if (String.IsNullOrEmpty(serverName))
      {
        if (checks.All(e => e.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy))
        {
          return Color.Error;
        }
        else if (checks.Any(e => e.Status is Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded or Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy))
        {
          return Color.Warning;
        }
        else if (checks.All(e => e.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy))
        {
          return Color.Success;
        }
        return Color.Neutral;
      }
      else
      {
        return checks.SelectMany(e => e.Checks).FirstOrDefault(e => e.ServerName == serverName).Status switch
        {
          Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy => Color.Success,
          Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded => Color.Warning,
          Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy => Color.Error,
          _ => Color.Neutral
        };
      }
    }
    #endregion

    #region " --> GetStatusIcon                           "
    public static Icon GetStatusIcon(HealthCheckHistoryEntry historyEntry) => historyEntry.Status switch
    {
      Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Checkmark(),
      Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Warning(),
      Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.ErrorCircle(),
      _ => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Question()
    };
    public static Icon GetStatusIcon(IEnumerable<HealthCheckStateView> checks, String serverName = "")
    {
      if (String.IsNullOrEmpty(serverName))
      {
        if (checks.All(e => e.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy))
        {
          return new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.ErrorCircle();
        }
        else if (checks.Any(e => e.Status is Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded or Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy))
        {
          return new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Warning();
        }
        else if (checks.All(e => e.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy))
        {
          return new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Checkmark();
        }
        return new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Question();
      }
      else
      {
        return checks.SelectMany(e => e.Checks).FirstOrDefault(e => e.ServerName == serverName).Status switch
        {
          Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Checkmark(),
          Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Warning(),
          Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.ErrorCircle(),
          _ => new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Question()
        };
      }
    }
    #endregion

    #endregion
  }
}

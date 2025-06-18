using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sample.Monitoring.Infrastructure.Health;
using Sample.Monitoring.Model.Health;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sample.Monitoring.API.Controllers
{
  /// <summary>
  /// API-Controller zur Überwachung und Verwaltung von Health-Checks für Anwendungen.
  /// </summary>
  [ApiController]
  [Route("api/[controller]/[action]")]
  public class HealthMonitorController(HealthCheckDbContext context) : ControllerBase
  {
    #region " Variablen/ Properties                       "

    #region " --> Context                                 "
    /// <summary>
    /// Der Datenbankkontext für Health-Check-Operationen.
    /// </summary>
    private readonly HealthCheckDbContext _Context = context;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> GetApplicationStatus                    "
    /// <summary>
    /// Gibt den aktuellen Status aller Health-Checks für eine bestimmte Anwendung zurück.
    /// </summary>
    /// <param name="applicationId">Die eindeutige ID der Anwendung.</param>
    /// <returns>Eine Liste von <see cref="HealthCheckStateView"/>-Objekten mit Statusinformationen.</returns>
    [HttpGet]
    public async Task<IEnumerable<HealthCheckStateView>> GetApplicationStatus(Guid applicationId)
    {
      IEnumerable<HealthCheckEntry> checkEntries = await this._Context.HealthCheckEntries.Include("HealthCheck").Where(e => e.ApplicationId == applicationId).ToListAsync();
      IEnumerable<HealthCheckStateView> states = [.. checkEntries
          .Select(e => new HealthCheckStateView()
          {
            FirstLevel = e.HealthCheck.Tags.FirstOrDefault(),
            SecondLevel = e.HealthCheck.Tags.LastOrDefault(),
            CheckName = e.Name
          })
          .Distinct()];

      _ = states.ForAll(state => state.Checks = [.. checkEntries.Where(e => e.Name.Equals(state.CheckName))]);

      return [.. states];
    }
    #endregion

    #region " --> GetHealthCheck                          "
    /// <summary>
    /// Gibt den aktuellen Status eines bestimmten Health-Checks für eine Anwendung zurück.
    /// </summary>
    /// <param name="applicationId">Die eindeutige ID der Anwendung.</param>
    /// <param name="checkName">Der Name des Health-Checks.</param>
    /// <returns>Ein <see cref="HealthCheckStateView"/>-Objekt mit Statusinformationen oder <c>null</c>, falls nicht gefunden.</returns>
    [HttpGet]
    public async Task<HealthCheckStateView> GetHealthCheck(Guid applicationId, String checkName)
    {
      try
      {
        IEnumerable<HealthCheckEntry> checkEntries = await this._Context.HealthCheckEntries
          .Include("HealthCheck")
          .Include("HistoryEntries")
          .Where(e => e.ApplicationId == applicationId && e.Name == checkName)
          .ToListAsync();

        IEnumerable<HealthCheckStateView> states = [.. checkEntries
          .Select(e => new HealthCheckStateView()
          {
            FirstLevel = e.HealthCheck.Tags.FirstOrDefault(),
            SecondLevel = e.HealthCheck.Tags.LastOrDefault(),
            CheckName = e.Name
          })
          .Distinct()];

        _ = states.ForAll(state => state.Checks = [.. checkEntries.Where(e => e.Name.Equals(state.CheckName))]);

        return states.FirstOrDefault();
      }
      catch (Exception ex)
      {
        System.Diagnostics.Log.Error(default, exception: ex);
        return default;
      }
    }
    #endregion

    #region " --> RegisterHealthCheck                     "
    /// <summary>
    /// Registriert einen neuen Health-Check in der Datenbank.
    /// </summary>
    /// <param name="healthCheck">Das zu registrierende <see cref="HealthCheck"/>-Objekt.</param>
    /// <returns>Ein <see cref="IActionResult"/> mit dem Ergebnis der Registrierung.</returns>
    [HttpPost]
    public async Task<IActionResult> RegisterHealthCheck(HealthCheck healthCheck)
    {
      try
      {
        return this.Ok(await this._Context.RegisterHealthCheckAsync(healthCheck));
      }
      catch (Exception ex)
      {
        System.Diagnostics.Log.Error(default, exception: ex);
        return this.Problem(ex.Message);
      }
    }
    #endregion

    #region " --> AddHealthCheck                          "
    /// <summary>
    /// Fügt einen neuen Health-Check-Eintrag für eine Anwendung hinzu.
    /// </summary>
    /// <param name="healthCheckEntry">Der hinzuzufügende <see cref="HealthCheckEntry"/>.</param>
    /// <returns>Ein <see cref="IActionResult"/> mit dem Ergebnis der Operation.</returns>
    [HttpPost]
    public async Task<IActionResult> AddHealthCheck(HealthCheckEntry healthCheckEntry)
    {
      try
      {
        await this._Context.AddHealthCheckAsync(healthCheckEntry);
        return this.Ok();
      }
      catch (Exception ex)
      {
        System.Diagnostics.Log.Error(default, exception: ex);
        return this.Problem(ex.Message);
      }
    }
    #endregion

    #endregion
  }
}

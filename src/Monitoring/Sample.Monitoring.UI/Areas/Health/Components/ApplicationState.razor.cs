using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Sample.Monitoring.Model.Health;
using Sample.UI.Extensions.Components;

namespace Sample.Monitoring.UI.Areas.Health.Components
{
  /// <summary>
  /// Blazor-Komponente zur Anzeige des aktuellen Anwendungsstatus und der Health-Checks.
  /// </summary>
  public partial class ApplicationState : BaseComponent
  {
    #region " Variablen/ Properties                       "

    #region " --> ServerNames                             "
    /// <summary>
    /// Liste der eindeutigen Servernamen, auf denen Health-Checks ausgeführt werden.
    /// </summary>
    String[] ServerNames = [];
    #endregion

    #region " --> States                                  "
    /// <summary>
    /// Auflistung der aktuellen Health-Check-Zustände, gruppiert nach Hierarchieebenen.
    /// </summary>
    IEnumerable<HealthCheckStateView> States = [];
    #endregion

    #endregion

    #region " interne Methoden                            "

    #region " --> OnInitializedCoreAsync                  "
    /// <summary>
    /// Lädt beim Initialisieren der Komponente die aktuellen Health-Check-Zustände
    /// und extrahiert die zugehörigen Servernamen.
    /// </summary>
    /// <returns>Ein <see cref="Task"/>, das den asynchronen Initialisierungsvorgang darstellt.</returns>
    protected override async Task OnInitializedCoreAsync()
    {
      try
      {
        this.States = await this.HealthStatusService.GetHealthStatusAsync();
        this.ServerNames = this.States?.SelectMany(e => e.Checks.Select(c => c.ServerName)).Distinct().Order().ToArray();
      }
      catch (Exception ex)
      {
        this.IsLoading = false;
        this.StateHasChanged();
        Log.Error(this.Context, ex, "Fehler beim Abrufen des Anwendungsstatus".AsMemory());
        //TODO: Fehlermeldung an den Benutzer
      }
    }
    #endregion

    #endregion
  }
}

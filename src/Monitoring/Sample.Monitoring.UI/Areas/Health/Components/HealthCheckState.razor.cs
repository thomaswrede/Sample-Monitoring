using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using Sample.Monitoring.Model.Health;
using Sample.UI.Extensions.Components;

namespace Sample.Monitoring.UI.Areas.Health.Components
{
  public partial class HealthCheckState : BaseComponent
  {
    #region " Variablen/ Properties                       "

    #region " --> CheckName                               "
    [SupplyParameterFromQuery(Name = "check")]
    public String CheckName { get; set; }
    #endregion

    #region " --> HealthCheck                             "
    public HealthCheckStateView HealthCheck { get; set; }
    #endregion

    #endregion

    #region " interne Methoden                            "

    #region " --> OnInitializedCoreAsync                  "
    protected override async Task OnInitializedCoreAsync()
    {
      try
      {
        this.HealthCheck = await this.HealthStatusService.GetHealthCheckAsync(this.CheckName);
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

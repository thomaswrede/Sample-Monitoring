using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace Sample.UI.Extensions.Components
{
  /// <summary>
  /// Basiskomponente für alle Komponenten in der bee.UI-Anwendung.
  /// </summary>
  public class BaseComponent : ComponentBase
  {
    const IUniqueIdentity IDENTITY = null; // Placeholder for the actual unique identity implementation

    #region " Variablen/ Properties                       "

    #region " --> IsReady                                 "
    /// <summary>
    /// Gibt an, ob die Komponente bereit ist.
    /// </summary>
    public Boolean IsReady { get; set; }
    #endregion

    #region " --> IsLoading                               "
    /// <summary>
    /// Gibt an, ob die Komponente gerade geladen wird.
    /// </summary>
    public Boolean IsLoading { get; set; }
    #endregion

    #region " --> Navigation                              "
    [Inject]
    protected NavigationManager Navigation { get; set; }
    #endregion

    #region " --> Context                                 "
    /// <summary>
    /// Gibt den Kontext der Komponente zurück.
    /// </summary>
    public IContext Context => this._Context ??= new Context(Guid.NewGuid().ToString(), IDENTITY);
    private IContext _Context;
    #endregion

    #endregion

    #region " Konstruktor                                 "

    #region " --> New                                     "
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="BaseComponent"/> Klasse.
    /// </summary>
    public BaseComponent() { }

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="BaseComponent"/> Klasse mit den angegebenen Abhängigkeiten.
    /// </summary>
    /// <param name="navigation">Der NavigationManager.</param>
    public BaseComponent(NavigationManager navigation) => this.Navigation = navigation;
    #endregion

    #endregion

    #region " interne Methoden                            "

    #region " --> OnInitializedAsync                      "
    /// <summary>
    /// Wird aufgerufen, wenn die Komponente initialisiert wird.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
      try
      {
        await this.OnInitializedCoreAsync();
        this.StateHasChanged();
        await base.OnInitializedAsync();
        this.IsReady = true;
      }
      catch (Exception ex)
      {
        Log.Error(this.Context, ex, "Fehler beim Initialisieren der Komponente".AsMemory());
        //TODO: Fehlermeldung an den Benutzer
      }
    }
    #endregion

    #region " --> OnInitializedCore/Async                 "
    /// <summary>
    /// Wird aufgerufen, wenn die Kerninitialisierung der Komponente stattfindet.
    /// </summary>
    protected virtual async Task OnInitializedCoreAsync() => await Task.CompletedTask;
    #endregion

    #endregion

  }
}

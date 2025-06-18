using System.ComponentModel.DataAnnotations;

using Sample.Extensions.Properties;

namespace System
{
  #region " Enumerations                                  "

  #region " --> ResultState                               "
  /// <summary>
  /// Enumeration zur Angabe des Erfolgsstatus einer Operation
  /// </summary>
  public enum ResultState : Int32
  {
    /// <summary>
    /// Initial-Status
    /// </summary>
    [Display(Name = "ResultState.None", ResourceType = typeof(EnumValueResources))]
    None = 0,
    /// <summary>
    /// Operation OK
    /// </summary>
    [Display(Name = "ResultState.Ok", ResourceType = typeof(EnumValueResources))]
    Ok = 1,
    /// <summary>
    /// Für die Operation ist eine Warnung angegeben
    /// </summary>
    [Display(Name = "ResultState.Warning", ResourceType = typeof(EnumValueResources))]
    Warning = 2,
    /// <summary>
    /// Operation ist Fehlgeschlagen
    /// </summary>
    [Display(Name = "ResultState.Error", ResourceType = typeof(EnumValueResources))]
    Error = 4,
    /// <summary>
    /// Bei der Operation ist ein schwerwiegender Fehler aufgetreten
    /// </summary>
    [Display(Name = "ResultState.Critical", ResourceType = typeof(EnumValueResources))]
    Critical = Int32.MaxValue
  }
  #endregion

  #endregion
}


using System.ComponentModel.DataAnnotations;

namespace System
{
  /// <summary>
  /// Definitiert ein Objekt das mithilfe eine Id eindeutig referenziert werden kann.
  /// </summary>
  public interface IUnique
  {
    #region " Properties                                  "

    #region " --> Id                                      "
    /// <summary>
    /// Ruft den eindeutigen Schlüssel der Objektinstanz ab.
    /// </summary>
    [Key]
    Guid Id { get; }
    #endregion

    #endregion
  }
}

using System.Security.Principal;

namespace System
{
  /// <summary>
  /// Definiert einen Kontext mit Identität und Client-Informationen.
  /// </summary>
  public interface IContext
  {
    #region " Properties                                  "

    #region " --> Identity                                "
    /// <summary>
    /// Ruft die eindeutige Identität des aktuellen Kontexts ab.
    /// </summary>
    IUniqueIdentity Identity { get; }
    #endregion

    #region " --> ClientId                                "
    /// <summary>
    /// Ruft die Client-ID ab, die mit dem aktuellen Kontext verknüpft ist.
    /// </summary>
    String ClientId { get; }
    #endregion

    #endregion
  }
}

namespace System.Security.Principal
{
  /// <summary>
  /// Stellt eine Identität mit einer eindeutigen Kennung dar.
  /// Erweitert <see cref="IUnique"/> und <see cref="IIdentity"/>.
  /// </summary>
  public interface IUniqueIdentity : IUnique, IIdentity;
}

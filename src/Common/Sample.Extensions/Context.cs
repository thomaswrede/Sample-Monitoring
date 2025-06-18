using System;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace Sample
{
  /// <summary>
  /// Stellt den Kontext dar, der Informationen über die Identität, den Mandanten und andere Eigenschaften enthält.
  /// </summary>
  [Serializable]
  public class Context : IContext
  {
    #region " Variablen/ Properties                       "

    #region " --> Identity                                "
    /// <summary>
    /// Ruft die eindeutige Identität des Kontexts ab.
    /// </summary>
    [JsonPropertyName(nameof(Identity))]
    public IUniqueIdentity Identity { get; }
    #endregion

    #region " --> ClientId                                "
    /// <summary>
    /// Ruft die Client-ID des Kontexts ab.
    /// </summary>
    [JsonPropertyName(nameof(ClientId))]
    public String ClientId => this._ClientId.ToString();
    private readonly ReadOnlyMemory<Char> _ClientId;
    #endregion

    #endregion

    #region " Konstruktor                                 "

    #region " --> New                                     "
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="Context"/> Klasse.
    /// </summary>
    private Context() { }
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="Context"/> Klasse mit der angegebenen Client-ID, Identität und Mandanten.
    /// </summary>
    /// <param name="clientId">Die Client-ID des Kontexts.</param>
    /// <param name="identity">Die eindeutige Identität des Kontexts.</param>
    public Context(ReadOnlyMemory<Char> clientId, IUniqueIdentity identity)
    {
      this._ClientId = clientId;
      this.Identity = identity;
    }
    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="Context"/> Klasse mit der angegebenen Client-ID, Identität und Mandanten.
    /// </summary>
    /// <param name="clientId">Die Client-ID des Kontexts.</param>
    /// <param name="identity">Die eindeutige Identität des Kontexts.</param>
    public Context(String clientId, IUniqueIdentity identity) : this(clientId.AsMemory(), identity) { }
    #endregion

    #endregion
  }
}

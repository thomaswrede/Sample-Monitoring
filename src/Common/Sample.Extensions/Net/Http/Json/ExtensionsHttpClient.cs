using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Net.Http.Json
{
  /// <summary>
  /// Stellt Erweiterungsmethoden für <see cref="HttpClient"/> bereit, um JSON-basierte HTTP-Anfragen zu vereinfachen.
  /// </summary>
  public static class ExtensionsHttpClient
  {
    #region " öffentliche Methoden                        "

    #region " --> GetAsync                                "
    /// <summary>
    /// Sendet eine HTTP-GET-Anfrage an die angegebene URL und deserialisiert die JSON-Antwort in den angegebenen Typ.
    /// </summary>
    /// <typeparam name="TOut">Der Typ, in den die Antwort deserialisiert werden soll.</typeparam>
    /// <param name="client">Die <see cref="HttpClient"/>-Instanz.</param>
    /// <param name="url">Die Ziel-URL der Anfrage.</param>
    /// <returns>Das deserialisierte Objekt vom Typ <typeparamref name="TOut"/>.</returns>
    public static async Task<TOut> GetAsync<TOut>(this HttpClient client, String url)
    {
      HttpResponseMessage message = await client.GetAsync(url);
      _ = message.EnsureSuccessStatusCode();
      return await JsonSerializer.DeserializeAsync<TOut>(await message.Content.ReadAsStreamAsync());
    }
    #endregion

    #region " --> PostAsJsonAsync                         "
    /// <summary>
    /// Sendet eine HTTP-POST-Anfrage mit einem als JSON serialisierten Objekt an die angegebene URL.
    /// </summary>
    /// <typeparam name="T">Der Typ des zu sendenden Objekts.</typeparam>
    /// <param name="client">Die <see cref="HttpClient"/>-Instanz.</param>
    /// <param name="url">Die Ziel-URL der Anfrage.</param>
    /// <param name="value">Das zu serialisierende und zu sendende Objekt.</param>
    /// <returns>Die HTTP-Antwortnachricht.</returns>
    public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, String url, T value)
    {
      HttpContent content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
      return await client.PostAsync(url, content);
    }

    /// <summary>
    /// Sendet eine HTTP-POST-Anfrage mit einem als JSON serialisierten Objekt an die angegebene URL und deserialisiert die JSON-Antwort in den angegebenen Typ.
    /// </summary>
    /// <typeparam name="TIn">Der Typ des zu sendenden Objekts.</typeparam>
    /// <typeparam name="TOut">Der Typ, in den die Antwort deserialisiert werden soll.</typeparam>
    /// <param name="client">Die <see cref="HttpClient"/>-Instanz.</param>
    /// <param name="url">Die Ziel-URL der Anfrage.</param>
    /// <param name="value">Das zu serialisierende und zu sendende Objekt.</param>
    /// <returns>Das deserialisierte Objekt vom Typ <typeparamref name="TOut"/>.</returns>
    public static async Task<TOut> PostAsJsonAsync<TIn, TOut>(this HttpClient client, String url, TIn value)
    {
      HttpContent content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
      HttpResponseMessage message = await client.PostAsync(url, content);
      _ = message.EnsureSuccessStatusCode();

      return await JsonSerializer.DeserializeAsync<TOut>(await message.Content.ReadAsStreamAsync());
    }
    #endregion

    #region " --> PutAsJsonAsync                          "
    /// <summary>
    /// Sendet eine HTTP-PUT-Anfrage mit einem als JSON serialisierten Objekt an die angegebene URL und deserialisiert die JSON-Antwort in den angegebenen Typ.
    /// </summary>
    /// <typeparam name="TIn">Der Typ des zu sendenden Objekts.</typeparam>
    /// <typeparam name="TOut">Der Typ, in den die Antwort deserialisiert werden soll.</typeparam>
    /// <param name="client">Die <see cref="HttpClient"/>-Instanz.</param>
    /// <param name="url">Die Ziel-URL der Anfrage.</param>
    /// <param name="value">Das zu serialisierende und zu sendende Objekt.</param>
    /// <returns>Das deserialisierte Objekt vom Typ <typeparamref name="TOut"/>.</returns>
    public static async Task<TOut> PutAsJsonAsync<TIn, TOut>(this HttpClient client, String url, TIn value)
    {
      HttpContent content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
      HttpResponseMessage message = await client.PutAsync(url, content);
      _ = message.EnsureSuccessStatusCode();

      return await JsonSerializer.DeserializeAsync<TOut>(await message.Content.ReadAsStreamAsync());
    }
    #endregion

    #endregion

  }
}

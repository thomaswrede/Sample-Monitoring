using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Monitoring.Health
{
  /// <summary>
  /// Stellt Hilfsmethoden zum Schreiben von HealthCheck-Antworten im JSON-Format bereit.
  /// </summary>
  public static class HealthResponseWriter
  {
    #region " Konstanten                                  "

    #region " --> CONTENT_TYPE_JSON                       "
    /// <summary>
    /// Der Content-Type für JSON-Antworten.
    /// </summary>
    private const String CONTENT_TYPE_JSON = "application/json";
    #endregion

    #endregion

    #region " Variablen/ Properties                       "

    #region " --> EmptyResponse                           "
    /// <summary>
    /// Leere JSON-Antwort für den Fall, dass kein HealthReport vorliegt.
    /// </summary>
    private static readonly Byte[] _EmptyResponse = "{}"u8.ToArray();
    #endregion

    #region " --> Options                                 "
    /// <summary>
    /// Lazy-Initialisierung der <see cref="JsonSerializerOptions"/> für die Serialisierung.
    /// </summary>
    private static readonly Lazy<JsonSerializerOptions> _Options = new(CreateJsonOptions);
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> WriteHealthCheckResponse                "
    /// <summary>
    /// Schreibt die HealthCheck-Antwort als JSON in den HTTP-Response.
    /// </summary>
    /// <param name="httpContext">Der aktuelle <see cref="HttpContext"/>.</param>
    /// <param name="report">Der <see cref="HealthReport"/> mit den Health-Informationen.</param>
    /// <returns>Ein <see cref="Task"/>, das die asynchrone Operation repräsentiert.</returns>
    public static async Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport report)
    {
      httpContext.Response.StatusCode = 200;

      if (report != null)
      {
        httpContext.Response.ContentType = CONTENT_TYPE_JSON;
        await JsonSerializer.SerializeAsync(httpContext.Response.Body, report, _Options.Value).ConfigureAwait(false);
      }
      else
      {
        _ = await httpContext.Response.BodyWriter.WriteAsync(_EmptyResponse).ConfigureAwait(false);
      }
    }
    #endregion

    #endregion

    #region " interne Methoden                            "

    #region " --> CreateJsonOptions                       "
    /// <summary>
    /// Erstellt und konfiguriert die <see cref="JsonSerializerOptions"/> für die HealthCheck-Serialisierung.
    /// </summary>
    /// <returns>Die konfigurierten <see cref="JsonSerializerOptions"/>.</returns>
    private static JsonSerializerOptions CreateJsonOptions()
    {
      JsonSerializerOptions options = new()
      {
        AllowTrailingCommas = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
      };

      options.Converters.Add(new JsonStringEnumConverter());
      options.Converters.Add(new TimeSpanConverter());
      options.Converters.Add(new ExceptionConverter());

      return options;
    }
    #endregion

    #endregion

    /// <summary>
    /// JSON-Konverter für <see cref="TimeSpan"/>, serialisiert als Zeichenkette.
    /// </summary>
    internal class TimeSpanConverter : JsonConverter<TimeSpan>
    {
      /// <inheritdoc/>
      public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => default;

      /// <inheritdoc/>
      public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
    }

    /// <summary>
    /// JSON-Konverter für <see cref="Exception"/>, serialisiert nur die Message.
    /// </summary>
    internal class ExceptionConverter : JsonConverter<Exception>
    {
      /// <inheritdoc/>
      public override Exception Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => default;

      /// <inheritdoc/>
      public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options) => writer.WriteStringValue(value.Message);
    }
  }
}

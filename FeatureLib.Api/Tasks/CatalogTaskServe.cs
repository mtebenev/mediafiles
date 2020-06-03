using EmbedIO;
using EmbedIO.Utilities;
using EmbedIO.WebApi;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.FeatureLib.Api.Controllers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mt.MediaFiles.FeatureLib.Api.Tasks
{
  /// <summary>
  /// Starts the API server.
  /// </summary>
  public sealed class CatalogTaskServe : CatalogTaskBase<int>
  {
    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task<int> ExecuteAsync(ICatalogContext catalogContext)
    {
      using(var server = CreateWebServer())
      {
        await server.RunAsync();
      }
      return 0;
    }

    /// <summary>
    /// Configures the web server.
    /// </summary>
    private WebServer CreateWebServer()
    {
      var url = "http://localhost:9001/";
      var server = new WebServer(o => o
        .WithUrlPrefix(url)
        .WithMode(HttpListenerMode.EmbedIO))
        .WithCors()
        .WithWebApi(
          "/api",
          SerializationCallback,
          config => config
            .WithController<ItemController>());

      return server;
    }

    /// <summary>
    /// TODO: Replace with built-int convertor when available:
    /// https://github.com/unosquare/embedio/pull/468
    /// </summary>
    private static async Task SerializationCallback(IHttpContext context, object? data)
    {
      Validate.NotNull(nameof(context), context).Response.ContentType = MimeType.Json;
      using var text = context.OpenResponseText(new UTF8Encoding(false));
      await text.WriteAsync(JsonSerializer.Serialize(data, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })).ConfigureAwait(false);
    }
  }
}

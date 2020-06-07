using EmbedIO;
using EmbedIO.Utilities;
using EmbedIO.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.FeatureLib.Api.Controllers;
using System;
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
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogTaskServe(IServiceProvider serviceProvider)
    {
      this._serviceProvider = serviceProvider;
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task<int> ExecuteAsync(ICatalogContext catalogContext)
    {
      using(var server = CreateWebServer(catalogContext))
      {
        await server.RunAsync();
      }
      return 0;
    }

    /// <summary>
    /// Configures the web server.
    /// </summary>
    private WebServer CreateWebServer(ICatalogContext catalogContext)
    {
      var url = "http://localhost:9001/";
      var server = new WebServer(o => o
        .WithUrlPrefix(url)
        .WithMode(HttpListenerMode.EmbedIO))
        .WithCors()
        .WithWebApi(
          "/api",
          SerializationCallback,
          config => RegisterControllers(config, catalogContext));

      return server;
    }

    /// <summary>
    /// TODO: Replace with built-int convertor when available:
    /// https://github.com/unosquare/embedio/pull/468
    /// </summary>
    private static async Task SerializationCallback(IHttpContext context, object data)
    {
      Validate.NotNull(nameof(context), context).Response.ContentType = MimeType.Json;
      using var text = context.OpenResponseText(new UTF8Encoding(false));
      await text.WriteAsync(JsonSerializer.Serialize(data, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })).ConfigureAwait(false);
    }

    /// <summary>
    /// API controllers registration.
    /// </summary>
    private void RegisterControllers(WebApiModule module, ICatalogContext catalogContext)
    {
      module.RegisterController<ItemController>(() =>
      {
        var result = ActivatorUtilities.CreateInstance<ItemController>(this._serviceProvider, catalogContext);
        return result;
      });
      module.RegisterController<ThumbnailController>(() =>
      {
        var result = ActivatorUtilities.CreateInstance<ThumbnailController>(this._serviceProvider);
        return result;
      });
    }
  }
}

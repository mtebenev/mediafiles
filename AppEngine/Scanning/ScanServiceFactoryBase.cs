using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Base implementation for typical service factories.
  /// </summary>
  public abstract class ScanServiceFactoryBase<TService> : IScanServiceFactory where TService : IScanService
  {
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ScanServiceFactoryBase(IServiceProvider serviceProvider, string id, List<string> dependencyIds)
    {
      this._serviceProvider = serviceProvider;
      this.Id = id;
      this.Dependencies = dependencyIds;
    }

    /// <summary>
    /// IScanServiceFactory.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// IScanServiceFactory.
    /// </summary>
    public IReadOnlyList<string> Dependencies { get; }

    /// <summary>
    /// IScanServiceFactory.
    /// </summary>
    public IScanService Create()
    {
      var result = ActivatorUtilities.CreateInstance<TService>(this._serviceProvider);
      return result;
    }
  }
}

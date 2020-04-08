using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.FileHandlers;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Builds the scan configuration according to required settings.
  /// </summary>
  internal class ScanConfigurationBuilder : IScanConfigurationBuilder
  {
    private readonly Lazy<List<IFileHandler>> _fileHandlers;
    private readonly Lazy<List<IScanTask>> _scanTasks;
    private readonly IServiceProvider _serviceProvider;

    public ScanConfigurationBuilder(IServiceProvider serviceProvider)
    {
      this._serviceProvider = serviceProvider;
      this._fileHandlers = new Lazy<List<IFileHandler>>(() =>
      {
        var fileHandlerTypes = new[]
        {
          typeof(FileHandlerVideo)
        };
        var handlerList = fileHandlerTypes
          .Select(x => ActivatorUtilities.CreateInstance(this._serviceProvider, x))
          .Cast<IFileHandler>()
          .ToList();

        return handlerList;
      }, LazyThreadSafetyMode.PublicationOnly);

      this._scanTasks = new Lazy<List<IScanTask>>(() =>
      {
        return new List<IScanTask>
        {
          new ScanTaskScanInfo()
        };
      });
    }

    /// <summary>
    /// IScanConfigurationBuilder.
    /// </summary>
    public Task<IScanConfiguration> BuildAsync(ScanParameters scanParameters, MmConfig mmConfig)
    {
      var fileHandlers = scanParameters.FileHandlerIds
        .Select(id => this._fileHandlers.Value.First(fh => fh.Id == id))
        .ToList();

      var subTasks = scanParameters.ScanTaskIds
        .Select(id => this._scanTasks.Value.First(st => st.Id == id))
        .ToList();

      var configuration =
        new ScanConfiguration(scanParameters, mmConfig)
      {
        FileHandlers = fileHandlers,
        ScanTasks = subTasks
      };

      return Task.FromResult<IScanConfiguration>(configuration);
    }
  }
}

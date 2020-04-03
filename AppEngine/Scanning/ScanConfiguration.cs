using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.FileHandlers;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ScanConfiguration : IScanConfiguration
  {
    private readonly Lazy<List<IFileHandler>> _fileHandlers;
    private readonly Lazy<List<ISubTask>> _subTasks;
    private readonly MmConfig _mmConfig;
    private readonly IServiceProvider _serviceProvider;

    public ScanConfiguration(string scanRootItemName, MmConfig mmConfig, IServiceProvider serviceProvider)
    {
      this.ScanRootItemName = scanRootItemName;
      this._serviceProvider = serviceProvider;
      this._mmConfig = mmConfig;
      this._fileHandlers = new Lazy<List<IFileHandler>>(() =>
      {
        var fileHandlerTypes = new[]
        {
          typeof(FileHandlerVideo),
          typeof(FileHandlerEpub)
        };
        var handlerList = fileHandlerTypes
          .Select(x => ActivatorUtilities.CreateInstance(this._serviceProvider, x))
          .Cast<IFileHandler>()
          .ToList();

        return handlerList;
      }, LazyThreadSafetyMode.PublicationOnly);

      this._subTasks = new Lazy<List<ISubTask>>(() =>
      {
        return new List<ISubTask>
        {
          new SubTaskScanInfo()
        };
      });
    }

    /// <summary>
    /// IScanConfiguration.
    /// </summary>
    public string ScanRootItemName { get; }

    /// <summary>
    /// IScanConfiguration.
    /// </summary>
    public IReadOnlyList<IFileHandler> FileHandlers => this._fileHandlers.Value;

    /// <summary>
    /// IScanConfiguration.
    /// </summary>
    public IReadOnlyList<ISubTask> SubTasks => this._subTasks.Value;

    /// <summary>
    /// IScanConfiguration
    /// </summary>
    public bool IsIgnoredEntry(string entryName)
    {
      var result = _mmConfig.Ignore.Any(fn => fn.Equals(entryName, StringComparison.InvariantCultureIgnoreCase));
      return result;
    }
  }
}

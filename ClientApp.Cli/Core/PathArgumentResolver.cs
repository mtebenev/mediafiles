using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.ClientApp.Cli.Core
{
  internal class PathArgumentResolver : IPathArgumentResolver
  {
    private readonly IFileSystem _fileSystem;
    private readonly ICatalogSettings _catalogSettings;

    /// <summary>
    /// Ctor.
    /// </summary>
    public PathArgumentResolver(IFileSystem fileSystem, ICatalogSettings catalogSettings)
    {
      this._fileSystem = fileSystem;
      this._catalogSettings = catalogSettings;
    }

    /// <summary>
    /// IPathArgumentResolver.
    /// </summary>
    public IList<string> ResolveMany(string pathOrAlias)
    {
      string rootDir;
      if(string.IsNullOrEmpty(pathOrAlias))
      {
        rootDir = this._fileSystem.Directory.GetCurrentDirectory();
      }
      else
      {
        var mediaRoot = this._catalogSettings.MediaRoots
          .FirstOrDefault(mr => mr.Key.Equals(pathOrAlias, StringComparison.InvariantCultureIgnoreCase));
        if(mediaRoot.Key != null)
          rootDir = mediaRoot.Value;
        else
        {
          rootDir = this._fileSystem.Path.IsPathFullyQualified(pathOrAlias)
            ? pathOrAlias
            : this._fileSystem.Path.GetFullPath(pathOrAlias);
        }
      }

      if(!this._fileSystem.Directory.Exists(rootDir))
        throw new InvalidOperationException($"Could not find directory or media root: \"{rootDir}\"");

      var result = this._fileSystem.Directory
        .EnumerateFiles(rootDir)
        .ToList();

      return result;
    }
  }
}

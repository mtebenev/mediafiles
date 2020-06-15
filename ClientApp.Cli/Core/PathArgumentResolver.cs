using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.ClientApp.Cli.Core
{
  internal class PathArgumentResolver : IPathArgumentResolver
  {
    private readonly IFileSystem _fileSystem;

    /// <summary>
    /// Ctor.
    /// </summary>
    public PathArgumentResolver(IFileSystem fileSystem)
    {
      this._fileSystem = fileSystem;
    }

    /// <summary>
    /// IPathArgumentResolver.
    /// </summary>
    public IList<string> ResolveMany(string pathOrAlias, ICatalogSettings catalogSettings)
    {
      string rootDir;
      if(string.IsNullOrEmpty(pathOrAlias))
      {
        rootDir = this._fileSystem.Directory.GetCurrentDirectory();
      }
      else
      {
        var mediaRoot = catalogSettings.MediaRoots
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
        .EnumerateFiles(rootDir, "*", SearchOption.AllDirectories)
        .ToList();

      return result;
    }
  }
}

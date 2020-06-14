
using Mt.MediaFiles.AppEngine.CatalogStorage;
using System.Collections.Generic;

namespace Mt.MediaFiles.ClientApp.Cli.Core
{
  /// <summary>
  /// Resolves paths for command arguments.
  /// </summary>
  internal interface IPathArgumentResolver
  {
    /// <summary>
    /// Resolves file paths from a command's pathalias argument.
    /// </summary>
    IList<string> ResolveMany(string pathOrAlias, ICatalogSettings catalogSettings);
  }
}

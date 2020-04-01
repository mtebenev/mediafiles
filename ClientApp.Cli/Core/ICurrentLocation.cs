using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.ClientApp.Cli.Core
{
  /// <summary>
  /// The current catalog location.
  /// </summary>
  internal interface ICurrentLocation
  {
    /// <summary>
    /// Loads children in the current level.
    /// </summary>
    Task<IList<ICatalogItem>> GetChildrenAsync();

    /// <summary>
    /// Switch location to the given item (by name or id)
    /// </summary>
    Task ChangeAsync(string itemNameOrId);

    /// <summary>
    /// Creates prompt for the current location.
    /// </summary>
    string CreatePrompt();
  }
}

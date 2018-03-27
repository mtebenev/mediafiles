using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Not sure if it's a bug but subcommand option values are not re-build if executed multiple time.
  /// Thus we are re-creating application each time and keep current catalog item in the context
  /// </summary>
  internal class ShellContext
  {
    public ShellContext(ICatalogItem initialItem)
    {
      CurrentItem = initialItem;
    }

    /// <summary>
    /// Get/set current item for navigation
    /// </summary>
    public ICatalogItem CurrentItem { get; set; }
  }
}

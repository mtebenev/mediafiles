using Microsoft.ReactNative.Managed;

namespace Mt.MediaMan.ArtefactUi.AppCoreLib.MediaExplorer
{
  /// <summary>
  /// The media explorer module.
  /// </summary>
  [ReactModule]
  internal class MediaExplorerModule
  {
    [ReactMethod("getFolders")]
    public void GetFolders(IReactPromise<string[]> promise)
    {
      var result = new string[]
      {
        "Item 1",
        "Item 2",
        "Item 3"
      };
      promise.Resolve(result);
    }
  }
}

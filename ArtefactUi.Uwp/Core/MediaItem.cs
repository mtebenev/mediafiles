using Windows.Storage;

namespace Mt.MediaMan.ArtefactUi.Uwp.Core
{
  /// <summary>
  /// TODO: To be removed
  /// </summary>
  public class MediaItem
  {
    private StorageFile _storageFile;
    public MediaItem(StorageFile storageFile)
    {
      this._storageFile = storageFile;
    }

    public StorageFile StorageFile => _storageFile;
  }
}

using System.IO;
using System.Threading.Tasks;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Defines an FS entry along with parent storage
  /// </summary>
  internal class FileStoreEntryContext
  {
    public FileStoreEntryContext(IFileStoreEntry fileStoreEntry, IFileStore fileStore)
    {
      FileStoreEntry = fileStoreEntry;
      FileStore = fileStore;
    }

    public IFileStoreEntry FileStoreEntry { get; }
    public IFileStore FileStore { get; }

    /// <summary>
    /// Shortcut for opening file stream
    /// </summary>
    public Task<Stream> GetFileStreamAsync()
    {
      return FileStore.GetFileStreamAsync(FileStoreEntry.Path);
    }
  }
}

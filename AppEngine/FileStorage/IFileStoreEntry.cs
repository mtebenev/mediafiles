using System;
using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.FileStorage
{
  /// <summary>
  /// Represents an abstract entry (file or directory) in a virtual file store.
  /// </summary>
  public interface IFileStoreEntry
  {
    /// <summary>
    /// Gets the full path of the file store entry within the file store.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Gets the name of the file store entry.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the full path of the file store entry's containing directory within the file store.
    /// </summary>
    string DirectoryPath { get; }

    /// <summary>
    /// Gets the length of the file (0 if the file story entry is a directory).
    /// </summary>
    long Length { get; }

    /// <summary>
    /// Gets the date and time in UTC when the file store entry was last modified.
    /// </summary>
    DateTime LastModifiedUtc { get; }

    /// <summary>
    /// Gets a boolean indicating whether the file store entry is a directory.
    /// </summary>
    bool IsDirectory { get; }

    /// <summary>
    /// Note MTE: this is dangerous, but we need to have access to physical paths
    /// Unlike Orchard some of 3-rd party APIs may work with file paths rather than with streams (i.e. MediaToolkit because of ffmpeg)
    /// </summary>
    Task<string> AccessFileAsync();
  }
}

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Mt.MediaMan.AppEngine.FileStorage
{
  public class FileSystemStoreEntry : IFileStoreEntry
  {
    private readonly IFileInfo _fileInfo;
    private readonly string _path;

    internal FileSystemStoreEntry(string path, IFileInfo fileInfo)
    {
      _fileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
      _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public string Path => _path;
    public string Name => _fileInfo.Name;
    public string DirectoryPath => _path.Substring(0, _path.Length - Name.Length).TrimEnd('/');
    public DateTime LastModifiedUtc => _fileInfo.LastModified.UtcDateTime;
    public long Length => _fileInfo.Length;
    public bool IsDirectory => _fileInfo.IsDirectory;

    /// <summary>
    /// Note MTE: make it not just string property because it may require unpacking etc
    /// </summary>
    public Task<string> AccessFileAsync()
    {
      return Task.FromResult(_fileInfo.PhysicalPath);
    }
  }
}

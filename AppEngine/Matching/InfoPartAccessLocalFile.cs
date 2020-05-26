using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.Scanning;
using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Info part access for local files.
  /// Should be instantiated with paths array. Indexes of the arrray are serving as item ids.
  /// </summary>
  public class InfoPartAccessLocalFile : IInfoPartAccess
  {
    private readonly IFileSystem _fileSystem;
    private readonly IMediaToolkitService _mediaToolkitService;
    private readonly string[] _paths;

    /// <summary>
    /// Ctor.
    /// </summary>
    public InfoPartAccessLocalFile(IFileSystem fileSystem, IMediaToolkitService mediaToolkitService, string[] paths)
    {
      this._fileSystem = fileSystem;
      this._mediaToolkitService = mediaToolkitService;
      this._paths = paths;
    }

    /// <summary>
    /// IInfoPartAccess.
    /// </summary>
    public Task<FileProperties> GetFilePropertiesAsync(int itemId)
    {
      var fileInfo = this._fileSystem.FileInfo.FromFileName(this._paths[itemId]);
      var result = new FileProperties
      {
        Path = this._paths[itemId],
        Size = fileInfo.Length
      };

      return Task.FromResult(result);
    }

    /// <summary>
    /// IInfoPartAccess.
    /// </summary>
    public async Task<TInfoPart> GetInfoPartAsync<TInfoPart>(int itemId) where TInfoPart : InfoPartBase
    {
      if(typeof(TInfoPart) != typeof(InfoPartVideo))
        throw new InvalidOperationException($"InfoPart {typeof(TInfoPart)} is not supported for local files.");

      var metaDataTask = new FfTaskGetMetadata(this._paths[itemId]);
      var taskResult = await this._mediaToolkitService.ExecuteAsync(metaDataTask);
      var videoStream = taskResult.Metadata.Streams.First(s => s.CodecType == "video");

      var result = new InfoPartVideo
      {
        Duration = (int)taskResult.Metadata.Format.Duration.TotalMilliseconds,
        VideoWidth = videoStream.Width,
        VideoHeight = videoStream.Height
      };

      return result as TInfoPart;
    }
  }
}

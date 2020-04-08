using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MediaToolkit.Model;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.FileHandlers;
using Mt.MediaFiles.AppEngine.FileStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.FileHandlers
{
  public class ScanDriverVideoTest
  {
    [Fact]
    public async Task Should_Extract_Video_Info()
    {
      var mockToolkitService = Substitute.For<IMediaToolkitService>();
      var mockScanContext = Substitute.For<IScanContext>();
      var mockStoreEntry = Substitute.For<IFileStoreEntry>();
      var mockFileStore = Substitute.For<IFileStore>();

      var entryContext = new FileStoreEntryContext(mockStoreEntry, mockFileStore);
      var itemData = new CatalogItemData(1);

      var ffProbeOutput = new FfProbeOutput
      {
        Format = new Format
        {
          Duration = "20",
          Tags = new Dictionary<string, string>
            {
              { "title", "video_title" }
            }
        },
        Streams = new List<MediaStream>
        {
          new MediaStream
          {
            CodecType = "video",
            Height = 300,
            Width = 400,
            CodecName = "codec",
            CodecLongName = "codec_long",
          }
        }
      };
      var taskResult = new GetMetadataResult(ffProbeOutput);

      mockToolkitService
        .ExecuteAsync(Arg.Any<FfTaskBase<GetMetadataResult>>())
        .Returns(taskResult);

      var driver = new ScanDriverVideo(mockToolkitService);
      await driver.ScanAsync(mockScanContext, 1, entryContext, itemData);

      var resultPart = itemData.Get<InfoPartVideo>();
      resultPart.Should()
        .BeEquivalentTo(
      new InfoPartVideo
      {
        Duration = 20,
        Title = "video_title",
        VideoHeight = 300,
        VideoWidth = 400,
        VideoCodecName = "codec",
        VideoCodecLongName = "codec_long"
      },
      options => options.Excluding(x => x.CatalogItemData));
    }
  }
}

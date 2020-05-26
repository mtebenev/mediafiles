using FluentAssertions;
using MediaToolkit.Model;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Scanning;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Matching
{
  public class InfoPartAccessLocalFileTest
  {
    [Fact]
    public async Task Return_File_Properties()
    {
      var paths = new[]
      {
        @"x:\dir\file.mp4"
      };
      var mockService = Substitute.For<IMediaToolkitService>();
      var mockFs = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
          { @"x:\dir\file.mp4", new MockFileData(new byte[] { 1, 2, 3 }) }
        });

      var access = new InfoPartAccessLocalFile(mockFs, mockService, paths);
      var fileProps = await access.GetFilePropertiesAsync(0);

      fileProps.Should().BeEquivalentTo(
        new FileProperties
        {
          Path = @"x:\dir\file.mp4",
          Size = 3
        });
    }

    [Fact]
    public async Task Return_Video_Info()
    {
      var paths = new[]
      {
        @"x:\dir\file.mp4"
      };
      var mockFs = Substitute.For<IFileSystem>();
      var mockService = Substitute.For<IMediaToolkitService>();

      var ffProbeOutput = new FfProbeOutput
      {
        Format = new Format
        {
          Duration = TimeSpan.FromMilliseconds(20)
        },
        Streams = new List<MediaStream>
        {
          new MediaStream
          {
            CodecType = "video",
            Height = 300,
            Width = 400,
          }
        }
      };
      var taskResult = new GetMetadataResult(ffProbeOutput);

      mockService
        .ExecuteAsync(Arg.Any<FfTaskBase<GetMetadataResult>>())
        .Returns(taskResult);

      var access = new InfoPartAccessLocalFile(mockFs, mockService, paths);
      var result = await access.GetInfoPartAsync<InfoPartVideo>(0);

      result.Should().BeEquivalentTo(
        new InfoPartVideo
        {
          VideoWidth = 400,
          VideoHeight = 300,
          Duration = 20
        });
    }
  }
}

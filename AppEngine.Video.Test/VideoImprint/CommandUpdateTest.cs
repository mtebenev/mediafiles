using System;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using FluentAssertions;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using NSubstitute;
using Xunit;

namespace AppEngine.Video.Test.VideoImprint
{
  public class CommandUpdateTest
  {
    [Fact]
    public async Task Should_Create_Imprints()
    {
      var catalogDef = @"
{
  name: 'Root',
  children: [
    {
      name: 'scan_root',
      rootPath: 'x:\\root_folder',
      children: [
        {
          name: 'folder1',
          children: [
            {
              id: 100,
              name: 'video1.mp4',
              fileSize: 3
            },
            {
              id: 200,
              name: 'video2.mp4',
              fileSize: 3
            }
          ]
        },
        {
          name: 'folder2',
          children: [
            {
              id: 300,
              name: 'video3.flv',
              fileSize: 3
            }
          ]
        }
    
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();

      var mockFs = new MockFileSystem();
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      var mockToolkitService = Substitute.For<IMediaToolkitService>();
      var mockContext = Substitute.For<ICommandExecutionContext>();
      mockContext.Catalog.Returns(mockCatalog);

      var mockServiceProvider = Substitute.For<IServiceProvider>();
      mockServiceProvider.GetService(typeof(IVideoImprintStorage)).Returns(mockStorage);
      mockServiceProvider.GetService(typeof(IMediaToolkitService)).Returns(mockToolkitService);

      var thumbResult = new GetThumbnailResult(new byte[0]);
      mockToolkitService.ExecuteAsync<GetThumbnailResult>(default).ReturnsForAnyArgs(thumbResult);

      var command = new CommandUpdate(mockFs, mockServiceProvider);
      await command.ExecuteAsync(mockContext, @"x:\folder");

      var args = mockStorage
        .ReceivedCalls()
        .SelectMany(x => x.GetArguments())
        .ToList();

      args.Should().BeEquivalentTo(new[]
      {
        new { CatalogItemId = 100 },
        new { CatalogItemId = 200 },
        new { CatalogItemId = 300 },
      });
    }
  }
}

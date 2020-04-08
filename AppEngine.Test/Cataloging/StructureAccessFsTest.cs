using System.IO.Abstractions;
using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Cataloging
{
  public class StructureAccessFsTest
  {
    [Fact]
    public async Task Should_Query_Level_Fs_Items()
    {
      var mockFs = Substitute.For<IFileSystem>();
      var mockItemStorage = Substitute.For<IItemStorage>();

      var location = new CatalogItemLocation(10, @"x:\folder1");
      var subTreeRecods = new[]
        {
          new CatalogItemRecord { CatalogItemId = 21, Path = @"x:\folder1\file1.mp4" },
          new CatalogItemRecord { CatalogItemId = 22, Path = @"x:\folder1\file2.mp4" },
          new CatalogItemRecord { CatalogItemId = 23, Path = @"x:\folder1\file3.mp4" },
        };
      mockItemStorage.QuerySubtree(location).Returns(subTreeRecods);


      var access = new StructureAccessFs(mockFs, mockItemStorage, 10);
      var records = await access.QueryLevelAsync(location);

      // Verify
      records.Should().BeEquivalentTo(subTreeRecods);
    }

    [Fact]
    public async Task Should_Query_Virtual_Folder_Records()
    {
      var mockFs = Substitute.For<IFileSystem>();
      var mockItemStorage = Substitute.For<IItemStorage>();

      var location = new CatalogItemLocation(10, @"x:\folder1");
      var subTreeRecods = new[]
        {
          new CatalogItemRecord { CatalogItemId = 21, Path = @"x:\folder1\folder2\file1.mp4" },
          new CatalogItemRecord { CatalogItemId = 22, Path = @"x:\folder1\folder2\file2.mp4" },
          new CatalogItemRecord { CatalogItemId = 23, Path = @"x:\folder1\folder3\file3.mp4" },
        };
      mockItemStorage.QuerySubtree(location).Returns(subTreeRecods);

      var access = new StructureAccessFs(mockFs, mockItemStorage, 10);
      var records = await access.QueryLevelAsync(location);

      // Verify
      records.Should().BeEquivalentTo(new[]
      {
          new CatalogItemRecord { ItemType = CatalogItemType.VirtualFolder, Path = @"folder2" },
          new CatalogItemRecord { ItemType = CatalogItemType.VirtualFolder, Path = @"folder3" },
      });
    }
  }
}

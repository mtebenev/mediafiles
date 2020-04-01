using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Scanning
{
  public class ItemExplorerFileSystemTest
  {
    [Fact]
    public async Task Should_Create_Records()
    {
      var mockFileInfo1 = Substitute.For<IFileInfo>();
      mockFileInfo1.FullName.Returns(@"x:\folder1\file1");
      mockFileInfo1.Length.Returns(10);

      var mockFileInfo2 = Substitute.For<IFileInfo>();
      mockFileInfo2.FullName.Returns(@"x:\folder2\file2");
      mockFileInfo2.Length.Returns(20);

      var mockDirectoryInfo = Substitute.For<IDirectoryInfo>();
      mockDirectoryInfo.EnumerateFiles(default, default)
        .ReturnsForAnyArgs(new[] { mockFileInfo1, mockFileInfo2 });

      var mockFs = Substitute.For<IFileSystem>();
      mockFs.DirectoryInfo.FromDirectoryName(@"x:\").Returns(mockDirectoryInfo);

      var sut = new ItemExplorerFileSystem(mockFs);
      var result = await sut.Explore(@"x:\", 1).ToListAsync();

      result.Should().BeEquivalentTo(new[]
      {
        new CatalogItemRecord {CatalogItemId = 0, Path = @"x:\folder1\file1", ParentItemId = 1, Size = 10, ItemType = CatalogItemType.File},
        new CatalogItemRecord {CatalogItemId = 0, Path = @"x:\folder2\file2", ParentItemId = 1, Size = 20, ItemType = CatalogItemType.File},
      });
    }

    [Fact]
    public async Task Should_Create_Scan_Root_Part()
    {
      var mockFileInfo1 = Substitute.For<IFileInfo>();
      mockFileInfo1.FullName.Returns(@"x:\folder1\file1");
      mockFileInfo1.Length.Returns(10);

      var mockDirectoryInfo = Substitute.For<IDirectoryInfo>();
      mockDirectoryInfo.EnumerateFiles(default, default)
        .ReturnsForAnyArgs(new[] { mockFileInfo1 });

      var mockDriveInfo = Substitute.For<IDriveInfo>();
      mockDriveInfo.Name.Returns(@"x:\");
      mockDriveInfo.DriveType.Returns(System.IO.DriveType.Fixed);

      var mockFs = Substitute.For<IFileSystem>();
      mockFs.DirectoryInfo.FromDirectoryName(@"x:\folder1\folder2").Returns(mockDirectoryInfo);
      mockFs.Path.GetPathRoot(@"x:\folder1\folder2").Returns(@"x:\");
      mockFs.DriveInfo.GetDrives().Returns(new[] { mockDriveInfo });

      var sut = new ItemExplorerFileSystem(mockFs);
      var result = await sut.CreateScanRootPartAsync(@"x:\folder1\folder2");

      result.Should().BeEquivalentTo(new InfoPartScanRoot
      {
        DriveType = "Fixed",
        RootPath = @"x:\folder1\folder2"
      });
    }
  }
}

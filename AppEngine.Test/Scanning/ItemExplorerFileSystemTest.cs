using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Scanning
{
  public class ItemExplorerFileSystemTest
  {
    [Fact]
    public async Task Should_Create_Records()
    {
      var mockScanConfiguration = Substitute.For<IScanConfiguration>();

      var mockFs = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
          { @"x:\folder1\file1", new MockFileData("12345") },
          { @"x:\folder2\file2", new MockFileData("123456789") },
        });

      var sut = new ItemExplorerFileSystem(mockFs);
      var result = await sut.Explore(@"x:\", 1, mockScanConfiguration).ToListAsync();

      result.Should().BeEquivalentTo(new[]
      {
        new CatalogItemRecord {CatalogItemId = 0, Path = @"x:\folder1\file1", ParentItemId = 1, Size = 5, ItemType = CatalogItemType.File},
        new CatalogItemRecord {CatalogItemId = 0, Path = @"x:\folder2\file2", ParentItemId = 1, Size = 9, ItemType = CatalogItemType.File},
      });
    }

    [Fact]
    public async Task Should_Use_Mmconfig_Ignore()
    {
      var mockScanConfiguration = Substitute.For<IScanConfiguration>();
      mockScanConfiguration.IsIgnoredEntry("folder3").Returns(true);

      var mockFs = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
          { @"x:\folder1\file1", new MockFileData("12345") },
          { @"x:\folder2\file2", new MockFileData("123456789") },
          { @"x:\folder3\ignored-file", new MockFileData("123456789") },
        });

      var sut = new ItemExplorerFileSystem(mockFs);
      var result = await sut.Explore(@"x:\", 1, mockScanConfiguration).ToListAsync();

      result.Should().BeEquivalentTo(new[]
      {
        new CatalogItemRecord {CatalogItemId = 0, Path = @"x:\folder1\file1", ParentItemId = 1, Size = 5, ItemType = CatalogItemType.File},
        new CatalogItemRecord {CatalogItemId = 0, Path = @"x:\folder2\file2", ParentItemId = 1, Size = 9, ItemType = CatalogItemType.File},
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

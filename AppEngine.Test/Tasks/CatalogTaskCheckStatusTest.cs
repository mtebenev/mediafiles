using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Tasks
{
  public class CatalogTaskCheckStatusTest
  {
    [Fact]
    public async Task Should_Compare_Files()
    {
      var catalogDef = @"
{
  path: 'Root',
  children: [
    {
      path: 'scan_root',
      rootPath: 'x:\\root_folder',
      id: 10,
      children: [
        { path: 'x:\\root_folder\\folder1\\folder2\\file1.txt', fileSize: 3 },
        { path: 'x:\\root_folder\\folder1\\folder2\\file2.txt', fileSize: 3 }
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();
      var mockCatalogContext = Substitute.For<ICatalogContext>();
      mockCatalogContext.Catalog.Returns(mockCatalog);
      var mockCi = await mockCatalog.GetItemByIdAsync(10);

      var mockFs = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
          { @"x:\root_folder\folder1\folder2\file1.txt", new MockFileData("abc") },
          { @"x:\root_folder\folder1\folder2\file2.txt", new MockFileData("abc") },
        });

      var catalogTask = new CatalogTaskCheckStatus(mockFs, mockCi);
      var result = await catalogTask.ExecuteTaskAsync(mockCatalogContext);
      var expected = new[]
      {
        new CheckStatusResult {Path = @"x:\root_folder\folder1\folder2\file1.txt", Status = FsItemStatus.Ok},
        new CheckStatusResult {Path = @"x:\root_folder\folder1\folder2\file2.txt", Status = FsItemStatus.Ok},
      };

      result.Should().BeEquivalentTo(expected,
        options => options.Excluding(p => p.CatalogItemId));
    }

    /// <summary>
    /// A path may contain spaces
    /// </summary>
    [Fact]
    public async Task Should_Work_With_Spaces_In_Paths()
    {
      var catalogDef = @"
{
  path: 'Root',
  children: [
    {
      path: 'scan_root',
      rootPath: 'x:\\root_folder',
      id: 10,
      children: [
        { path: 'x:\\root_folder\\folder 1\\folder 2\\file1.txt', fileSize: 3 },
        { path: 'x:\\root_folder\\folder 1\\folder 2\\file2.txt', fileSize: 3 }
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();
      var mockCatalogContext = Substitute.For<ICatalogContext>();
      mockCatalogContext.Catalog.Returns(mockCatalog);
      var mockCi = await mockCatalog.GetItemByIdAsync(10);

      var mockFs = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
          { @"x:\root_folder\folder 1\folder 2\file1.txt", new MockFileData("abc") },
          { @"x:\root_folder\folder 1\folder 2\file2.txt", new MockFileData("abc") },
        });

      var command = new CatalogTaskCheckStatus(mockFs, mockCi);
      var result = await command.ExecuteTaskAsync(mockCatalogContext);
      var expected = new[]
      {
        new CheckStatusResult {Path = @"x:\root_folder\folder 1\folder 2\file1.txt", Status = FsItemStatus.Ok},
        new CheckStatusResult {Path = @"x:\root_folder\folder 1\folder 2\file2.txt", Status = FsItemStatus.Ok},
      };

      result.Should().BeEquivalentTo(expected,
        options => options.Excluding(p => p.CatalogItemId));
    }
  }
}

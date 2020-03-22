using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using NSubstitute;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Commands
{
  public class CatalogTaskCheckStatusTest
  {
    [Fact]
    public async Task Should_Compare_Files()
    {
      var catalogDef = @"
{
  name: 'Root',
  children: [
    {
      name: 'root_folder',
      rootPath: 'x:\\root_folder',
      children: [
        {
          name: 'folder1',
          id: 10,
          children: [
            {
              name: 'folder2',
              children: [
                {
                  name: 'file1.txt',
                  fileSize: 3
                },
                {
                  name: 'file2.txt',
                  fileSize: 3
                },
              ]
            }
          ]
        }
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

      var command = new CatalogTaskCheckStatus(mockFs, mockCi);
      var result = await command.ExecuteTaskAsync(mockCatalogContext);
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
  name: 'Root',
  children: [
    {
      name: 'root_folder',
      rootPath: 'x:\\root_folder',
      children: [
        {
          name: 'folder 1',
          id: 10,
          children: [
            {
              name: 'folder 2',
              children: [
                {
                  name: 'file1.txt',
                  fileSize: 3
                },
                {
                  name: 'file2.txt',
                  fileSize: 3
                },
              ]
            }
          ]
        }
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

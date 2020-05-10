using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Tools;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Tasks
{
  public class CatalogTaskSearchDuplicatesTest
  {
    [Fact]
    public async Task Should_Find_Duplicates()
    {
      var catalogDef = @"
{
  path: 'Root',
  children: [
    {
      path: 'scan_root',
      rootPath: 'x:\\root_folder',
      children: [
        { id: 100, path: 'x:\\root_folder\\folder1\\file1.txt', fileSize: 3 },
        { id: 200, path: 'x:\\root_folder\\folder1\\file2.txt', fileSize: 3 },
        { id: 300, path: 'x:\\root_folder\\folder2\\file1.txt', fileSize: 3 }
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();

      var mockCatalogContext = Substitute.For<ICatalogContext>();
      mockCatalogContext.Catalog.Returns(mockCatalog);

      var catalogTask = new CatalogTaskSearchDuplicates();

      var result = await catalogTask.ExecuteTaskAsync(mockCatalogContext);
      Assert.Equal(1, result.Count);

      var expectedInfos = new List<BriefFileInfo>
      {
        new BriefFileInfo(100, @"x:\root_folder\folder1\file1.txt", 3),
        new BriefFileInfo(300, @"x:\root_folder\folder2\file1.txt", 3),
      };
      result[0].FileInfos.Should().BeEquivalentTo(expectedInfos);
    }
  }
}

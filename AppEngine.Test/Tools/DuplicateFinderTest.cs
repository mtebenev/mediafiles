using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using Mt.MediaMan.AppEngine.Tools;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Tools
{
  public class DuplicateFinderTest
  {
    [Fact]
    public async Task Should_Find_Duplicates()
    {
      var catalogDef = @"
{
  name: 'Root',
  rootPath: 'x:\\root_folder',
  children: [
    {
      name: 'folder1',
      children: [
        {
            id: 100,
            name: 'file1.txt',
            fileSize: 3
        },
        {
            id: 200,
            name: 'file2.txt',
            fileSize: 3
        }
      ]
    },
    {
      name: 'folder2',
      children: [
        {
            id: 300,
            name: 'file1.txt',
            fileSize: 3
        }
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();
      var duplicateFinder = new DuplicateFinder(mockCatalog);

      var result = await duplicateFinder.FindAsync();
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

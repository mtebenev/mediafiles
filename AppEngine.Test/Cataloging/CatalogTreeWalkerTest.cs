using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Cataloging
{
  public class CatalogTreeWalkerTest
  {
    [Fact]
    public async Task Enumerate_Items()
    {
      var catalogDef = @"
{
  name: 'Root',
  children: [
    {
      name: 'root_folder',
      children: [
        {
          name: 'folder1',
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
                }
              ]
            }
          ]
        },
        {
          name: 'folder1_2',
          children: [
            {
              name: 'file2_1'
            },
            {
              name: 'file2_2'
            }
          ]
        }
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();
      var walker = CatalogTreeWalker.CreateDefaultWalker(mockCatalog, 1);

      var names = await walker
        .Select(i => i.Path)
        .ToListAsync();

      var expected = new[] { "Root", "root_folder", "folder1", "folder1_2", "folder2", "file2_1", "file2_2", "file1.txt", "file2.txt" };
      Assert.Equal(expected, names);
    }
  }
}

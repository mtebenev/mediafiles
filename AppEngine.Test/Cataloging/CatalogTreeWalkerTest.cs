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
  path: 'Root',
  children: [
    {
      path: 'scan_root',
      children: [
        {
          path: 'folder1',
          children: [
            {
              path: 'folder2',
              children: [
                {
                  path: 'file1.txt',
                  fileSize: 3
                },
                {
                  path: 'file2.txt',
                  fileSize: 3
                }
              ]
            }
          ]
        },
        {
          path: 'folder1_2',
          children: [
            {
              path: 'file2_1'
            },
            {
              path: 'file2_2'
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

      var expected = new[] { "Root", "scan_root", "folder1", "folder1_2", "folder2", "file2_1", "file2_2", "file1.txt", "file2.txt" };
      Assert.Equal(expected, names);
    }
  }
}

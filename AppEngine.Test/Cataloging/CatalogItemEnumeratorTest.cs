using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using System;

namespace Mt.MediaMan.AppEngine.Test.Cataloging
{
  public class CatalogItemEnumeratorTest
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
      var enumerator = new CatalogItemEnumerator(mockCatalog, 1);

      var names = await enumerator
        .EnumerateAsync()
        .Select(i => i.Path)
        .ToListAsync();

      var expected = new[] { "Root", "scan_root", "folder1", "folder1_2", "folder2", "file2_1", "file2_2", "file1.txt", "file2.txt" };
      Assert.Equal(expected, names);
    }

    /// <summary>
    /// When enumerating logical structure, there are virtual folders for nested items.
    /// The enumerator should ignore them.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_Ignore_Virtual_Folders()
    {
      var catalogDef = @"
{
  path: 'Root',
  children: [
    {
      path: 'scan_root',
      rootPath: 'x:\\root_folder',
      children: [
        {
          path: 'folder1',
          id: 0,
          children: [
            { path: 'x:\\root_folder\\folder1\\file1.txt', fileSize: 3 },
            { path: 'x:\\root_folder\\folder1\\file2.txt', fileSize: 3 }
          ]
        }
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();
      mockCatalog.When(c => c.GetItemByIdAsync(0)).Throw<Exception>(); // When catalog actually asked to load an item with id=0, it throws.

      var enumerator = new CatalogItemEnumerator(mockCatalog, 1);

      var paths = await enumerator
        .EnumerateAsync()
        .Select(i => i.Path)
        .ToListAsync();

      var expected = new[]
      {
        "Root",
        "scan_root",
        "folder1",
        @"x:\root_folder\folder1\file1.txt",
        @"x:\root_folder\folder1\file2.txt"
      };
      Assert.Equal(expected, paths);
    }
  }
}

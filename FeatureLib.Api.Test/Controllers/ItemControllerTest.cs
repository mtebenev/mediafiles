using FluentAssertions;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.FeatureLib.Api.Controllers;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace FeatureLib.Api.Test.Controllers
{
  public class ItemControllerTest
  {
    [Fact]
    public async Task Get_Catalog_Items()
    {
      var catalogDef = @"
{
  path: 'Root',
  id: 10,
  children: [
    {
      path: 'scan_root',
      id: 11,
      rootPath: 'x:\\root_folder',
      children: [
        {
          path: 'folder1',
          id: 12,
          children: [
            { id: 13, path: 'x:\\root_folder\\folder1\\file1.txt', fileSize: 3 },
            { id: 14, path: 'x:\\root_folder\\folder1\\file2.txt', fileSize: 3 }
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

      var controller = new ItemController(mockCatalogContext);
      var result = await controller.Get();

      // Verify
      result.Should().BeEquivalentTo(
        new[]
        {
          new { CatalogItemId = 10, Path = "Root" },
          new { CatalogItemId = 11, Path = "scan_root" },
          new { CatalogItemId = 12, Path = "folder1" },
          new { CatalogItemId = 13, Path = @"x:\root_folder\folder1\file1.txt" },
          new { CatalogItemId = 14, Path = @"x:\root_folder\folder1\file2.txt" },
        });
    }
  }
}

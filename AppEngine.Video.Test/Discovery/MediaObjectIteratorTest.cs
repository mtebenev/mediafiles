using AppEngine.Video.Discovery;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using System.Threading.Tasks;
using Xunit;

namespace AppEngine.Video.Test
{
  public class MediaObjectIteratorTest
  {
    [Fact]
    public async Task Enumerate_Media_Ojbects()
    {
      var mockCatalog = CatalogMockBuilder
        .CreateDefault()
        .WithInfoPartVideo("Item 1.1", new InfoPartVideo { Title = "Title 1" })
        .WithInfoPartVideo("Item 2.1.1", new InfoPartVideo { Title = "Title 2" })
        .Build();

      var iterator = new MediaObjectIterator(mockCatalog, 1);
      
      // Act
      var mediaObjects = await iterator.GetMediaObjectsAsync();
      Assert.Equal(2, mediaObjects.Count);
    }
  }
}

using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.TestUtils
{
  /// <summary>
  /// Helper for building catalog item data mocks.
  /// </summary>
  public class CatalogItemDataMockBuilder
  {
    /// <summary>
    /// Creates a mock with a single video info part.
    /// </summary>
    public static CatalogItemData CreateVideoPart(int duration)
    {
      var infoPart = new InfoPartVideo
      {
        Duration = duration
      };

      var result = new CatalogItemData(0);
      result.Apply(infoPart);

      return result;
    }
  }
}

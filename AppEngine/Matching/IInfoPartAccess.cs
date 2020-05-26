using Mt.MediaFiles.AppEngine.Scanning;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Abstraction over info part access for catalog items.
  /// We need that in order to compare properties of catalog items and actual files.
  /// </summary>
  public interface IInfoPartAccess
  {
    /// <summary>
    /// Retrieves file properties of an item.
    /// </summary>
    Task<FileProperties> GetFilePropertiesAsync(int itemId);

    /// <summary>
    /// Retrieves an info part of an item.
    /// Throws if the info part could not be retrieved.
    /// </summary>
    Task<TInfoPart> GetInfoPartAsync<TInfoPart>(int itemId) where TInfoPart : InfoPartBase;
  }
}

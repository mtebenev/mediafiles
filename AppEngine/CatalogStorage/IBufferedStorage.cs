using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  /// <summary>
  /// Implement the interface if a storage works with data buffers.
  /// </summary>
  public interface IBufferedStorage
  {
    /// <summary>
    /// The storage should flush the data buffer.
    /// </summary>
    Task FlushAsync();
  }
}

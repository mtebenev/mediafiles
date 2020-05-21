using Dapper.Contrib.Extensions;

namespace Mt.MediaFiles.AppEngine.Video.Thumbnail
{
  /// <summary>
  /// The thumbnail record.
  /// </summary>
  [Table("VideoImprint")]
  public class ThumbnailRecord
  {
    public int ThumbnailId { get; set; }

    /// <summary>
    /// The related catalog item.
    /// </summary>
    public int CatalogItemId { get; set; }

    /// <summary>
    /// The offset in milliseconds.
    /// </summary>
    public int Offset { get; set; }
    public byte[] ThumbnailData { get; set; }
  }
}

using Dapper.Contrib.Extensions;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprint record.
  /// </summary>
  [Table("VideoImprint")]
  public class VideoImprintRecord
  {
    [Key]
    public int VideoImprintId { get; set; }

    public int CatalogItemId { get; set; }

    public byte[] ImprintData { get; set; }
  }
}

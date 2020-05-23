using Dapper.Contrib.Extensions;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  [Table("CatalogItem")]
  public sealed class CatalogItemRecord
  {
    [Key]
    public int CatalogItemId { get; set; }

    /// <summary>
    /// The path (or uri or whatever - depends on the item type and scanner) for the item.
    /// </summary>
    public string Path { get; internal set; }

    /// <summary>
    /// <see cref="Cataloging.CatalogItemType"/>
    /// </summary>
    public string ItemType { get; set; }

    /// <summary>
    /// Size in bytes for files.
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// The parent record id.
    /// </summary>
    public int ParentItemId { get; set; }
  }
}

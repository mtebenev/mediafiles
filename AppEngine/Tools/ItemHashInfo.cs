namespace Mt.MediaFiles.AppEngine.Tools
{
  /// <summary>
  /// Contains item ID along with hash
  /// </summary>
  internal class ItemHashInfo
  {
    public ItemHashInfo(int catalogItemId, int hash)
    {
      CatalogItemId = catalogItemId;
      Hash = hash;
    }

    public int CatalogItemId { get; }
    public int Hash { get; }
  }
}

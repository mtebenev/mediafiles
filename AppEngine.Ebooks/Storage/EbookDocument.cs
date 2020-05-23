namespace Mt.MediaFiles.AppEngine.Ebooks.Storage
{
  /// <summary>
  /// Contains information about ebook and stored in catalog
  /// </summary>
  public class EbookDocument
  {
    public string EbookDocumentId { get; set; }
    public string Title { get; set; }
    public string[] Authors { get; set; }
    public string Isbn { get; set; }
  }
}

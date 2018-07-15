using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Ebooks.Storage
{
  /// <summary>
  /// Stored in catalog item data and keeps track of related ebook
  /// </summary>
  internal class InfoPartEbookLink : InfoPartBase
  {
    public string EbookId { get; set; }
  }
}

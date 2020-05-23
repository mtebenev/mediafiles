using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Search
{
  /// <summary>
  /// Indexed values for a document (catalog item or other document)
  /// </summary>
  internal class DocumentIndex
  {
    public DocumentIndex(string itemId)
    {
      ItemId = itemId;
    }

    public string ItemId { get; }
    public Dictionary<string, DocumentIndexEntry> Entries { get; } = new Dictionary<string, DocumentIndexEntry>();

    public void Set(string name, string value, DocumentIndexOptions indexOptions)
    {
      Entries[name] = new DocumentIndexEntry(value, IndexValueType.Text, indexOptions);
    }
  }
}

namespace Mt.MediaMan.AppEngine.Search
{
  /// <summary>
  /// To be stored in lucene
  /// </summary>
  internal class DocumentIndexEntry
  {
    public DocumentIndexEntry(object value, IndexValueType indexValueType, DocumentIndexOptions indexOptions)
    {
      Value = value;
      IndexValueType = indexValueType;
      IndexOptions = indexOptions;
    }

    public object Value { get; }
    public IndexValueType IndexValueType { get; }
    public DocumentIndexOptions IndexOptions { get; }
  }
}

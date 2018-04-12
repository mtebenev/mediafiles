namespace Mt.MediaMan.AppEngine.Search
{
  /// <summary>
  /// To be stored in lucene
  /// </summary>
  internal class DocumentIndexEntry
  {
    public DocumentIndexEntry(object value, IndexValueType indexValueType)
    {
      Value = value;
      IndexValueType = indexValueType;
    }

    public object Value { get; }
    public IndexValueType IndexValueType { get; }
  }
}

using Lucene.Net.Index;
using LDirectory = Lucene.Net.Store.Directory;

namespace Mt.MediaMan.AppEngine.Search
{
  internal class IndexWriterWrapper : IndexWriter
  {
    public IndexWriterWrapper(LDirectory directory, IndexWriterConfig config) : base(directory, config)
    {
      IsClosing = false;
    }

    public bool IsClosing { get; set; }
  }
}

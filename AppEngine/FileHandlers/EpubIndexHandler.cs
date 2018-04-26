using System;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Indexing for epub catalog items
  /// </summary>
  internal class EpubIndexHandler : InfoPartIndexHandlerBase<InfoPartBook>
  {
    public override Task OnBuildIndexAsync(InfoPartBook part, IIndexingContext indexingContext)
    {
      throw new NotImplementedException();
      /*
      var documentIndexEntry = new DocumentIndexEntry(part.Title, IndexValueType.Text);
      indexingContext.DocumentIndex.Entries.Add(indexingContext.Key, documentIndexEntry);

      return Task.CompletedTask;
      */
    }
  }
}

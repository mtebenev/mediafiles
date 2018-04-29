using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Indexes Book info part
  /// </summary>
  internal class InfoPartIndexerBook : InfoPartIndexerBase<InfoPartBook>
  {
    public override Task OnBuildIndexAsync(InfoPartBook infoPart, IIndexingContext indexingContext)
    {
      indexingContext.DocumentIndex.Entries.Add(
        InfoPartBook.IndexField_Title,
        new DocumentIndexEntry(
          infoPart.Title,
          IndexValueType.Text,
          DocumentIndexOptions.Analyze));

      indexingContext.DocumentIndex.Entries.Add(
        InfoPartBook.IndexField_Authors,
        new DocumentIndexEntry(
          infoPart.GetAuthorsString(),
          IndexValueType.Text,
          DocumentIndexOptions.Analyze));

      return Task.CompletedTask;
    }
  }
}

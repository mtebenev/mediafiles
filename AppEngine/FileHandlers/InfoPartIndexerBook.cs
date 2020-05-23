using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Search;

namespace Mt.MediaFiles.AppEngine.FileHandlers
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

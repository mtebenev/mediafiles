using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using VersOne.Epub;

namespace Mt.MediaFiles.AppEngine.FileHandlers
{
  /// <summary>
  /// Extracts meta information from epub files
  /// </summary>
  internal class ScanDriverEpub : IScanDriver
  {
    public async Task ScanAsync(IScanContext scanContext, int catalogItemId, FileStoreEntryContext fileStoreEntryContext, CatalogItemData catalogItemData)
    {
      using(var fileStream = await fileStoreEntryContext.GetFileStreamAsync())
      {
        var ebook = await EpubReader.OpenBookAsync(fileStream);

        var infoPartBook = catalogItemData.GetOrCreate<InfoPartBook>();

        infoPartBook.Title = ebook.Title;
        infoPartBook.Authors = ebook.AuthorList.ToArray();

        catalogItemData.Apply(infoPartBook);
      }
    }
  }
}

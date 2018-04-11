using System;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using OrchardCore.FileStorage;
using VersOne.Epub;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Extracts meta information from epub files
  /// </summary>
  internal class ScanDriverEpub : IScanDriver
  {
    public Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry)
    {
      var supportedExtensions = new[] {".epub"};
      var result = supportedExtensions.Any(e => fileStoreEntry.Name.EndsWith(e, StringComparison.InvariantCultureIgnoreCase));

      return Task.FromResult(result);
    }

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

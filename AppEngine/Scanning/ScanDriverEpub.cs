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

    public async Task ScanAsync(IScanContext scanContext, int catalogItemId, FileStoreEntryContext fileStoreEntryContext, IItemStorage itemStorage)
    {
      using(var fileStream = await fileStoreEntryContext.GetFileStreamAsync())
      {
        var ebook = await EpubReader.OpenBookAsync(fileStream);

        var infoPartBook = new InfoPartBook
        {
          CatalogItemId = catalogItemId,
          Title = ebook.Title,
          Authors = ebook.AuthorList.ToArray()
        };

        await itemStorage.SaveInfoPartAsync(catalogItemId, infoPartBook);
      }
    }
  }
}

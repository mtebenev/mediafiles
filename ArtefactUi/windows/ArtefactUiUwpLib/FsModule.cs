using System;
using Microsoft.ReactNative.Managed;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using System.Linq;
using Windows.System.Threading;

namespace ArtefactUiUwpLib
{
  [ReactModule]
  internal class FsModule
  {
    [ReactMethod("getContents")]
    public async void GetContents(IReactPromise<string[]> promise)
    {
      StorageFolder folder;
      if(StorageApplicationPermissions.FutureAccessList.ContainsItem("ccc"))
      {
        folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("ccc");
      }
      else
      {
        ThreadPool.RunAsync(async (workItem) =>
        {
          var folderPicker = new Windows.Storage.Pickers.FolderPicker();
          folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
          folderPicker.FileTypeFilter.Add("*");
          folder = await folderPicker.PickSingleFolderAsync();
          int a = 4;
          a++;

        });

        /*
        var folderPicker = new Windows.Storage.Pickers.FolderPicker();
        folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
        folderPicker.FileTypeFilter.Add("*");

        //folder = await folderPicker.PickSingleFolderAsync();
        folderPicker.PickSingleFolderAsync().AsTask().Wait();
        */

        //StorageApplicationPermissions.FutureAccessList.AddOrReplace("ccc", folder);
      }

      //var files = await this.FillFileList(folder);
      //promise.Resolve(files);
    }
    private async Task<string[]> FillFileList(StorageFolder folder)
    {
      var items = await folder.GetItemsAsync();
      var result = items.Select(item => item.Name).ToArray();

      return result;
    }

  }
}

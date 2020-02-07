using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ReactNative.Managed;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace Mt.MediaMan.ArtefactUi.AppCoreLib.MediaExplorer
{
  class MediaLocation
  {
    public StorageFolder StorageFolder { get; set; }
  }

  class FileSystemItemDto
  {
    public string name { get; set; }
    public string path { get; set; }
    public bool isDirectory { get; set; }
  }

  class DirectoryContentDto
  {
    public FileSystemItemDto[] items { get; set; }
  }

  class MediaLocationDto
  {
    public string name { get; set; }
    public string path { get; set; }
  }

  /// <summary>
  /// The media explorer module.
  /// </summary>
  [ReactModule("MediaExplorerModule")]
  internal class MediaExplorerModule
  {
    private List<MediaLocation> _mediaLocations;

    public MediaExplorerModule()
    {
      this._mediaLocations = new List<MediaLocation>();
    }

    [ReactMethod("addLocation")]
    public void AddLocation(IReactPromise<bool> promise)
    {
      this.SelectFolder();
    }

    [ReactMethod("getMediaLocations")]
    public void GetMediaLocations(IReactPromise<MediaLocationDto[]> promise)
    {
      var result = _mediaLocations.Select(ml => new MediaLocationDto
      {
        name = ml.StorageFolder.Name,
        path = ml.StorageFolder.Path
      })
        .ToArray();
      promise.Resolve(result);
    }

    [ReactMethod("getRoot")]
    public void GetRoot(string name, IReactPromise<DirectoryContentDto> promise)
    {
      var mediaLocation = _mediaLocations.Find(ml => ml.StorageFolder.Name == name);
      if(mediaLocation != null)
      {
        var operation = mediaLocation.StorageFolder.GetItemsAsync();
        operation.AsTask().Wait();

        var fsItems = operation.GetResults();
        var items = fsItems.Select(fsi => new FileSystemItemDto
        {
          name = fsi.Name,
          path = fsi.Path,
          isDirectory = fsi.IsOfType(StorageItemTypes.Folder)
        }).ToArray();

        var contentDto = new DirectoryContentDto
        {
          items = items
        };

        promise.Resolve(contentDto);
      }
    }

    private void SelectFolder()
    {
      var folderPicker = new Windows.Storage.Pickers.FolderPicker();
      folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
      folderPicker.FileTypeFilter.Add("*");
      RunOnDispatcher(async () =>
      {
        var folder = await folderPicker.PickSingleFolderAsync();
        var mediaLocation = new MediaLocation
        {
          StorageFolder = folder
        };

        _mediaLocations.Add(mediaLocation);

        //promise.Resolve(10);
        //promise.Resolve(folder.Path);
        //AddEvent(folder.Path);
      });
      //AddEvent("sss");
    }

    private static async void RunOnDispatcher(DispatchedHandler action)
    {
      await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action).AsTask().ConfigureAwait(false);
    }
  }
}

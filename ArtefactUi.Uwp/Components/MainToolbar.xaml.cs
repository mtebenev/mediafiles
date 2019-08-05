using Mt.MediaMan.ArtefactUi.Uwp.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Mt.MediaMan.ArtefactUi.Uwp.Components
{
  public sealed partial class MainToolbar : UserControl
  {
    public MainToolbar()
    {
      this.InitializeComponent();
    }

    public MmAppContext AppContext { get; set; }

    private async void HandleOpenClick(object sender, RoutedEventArgs e)
    {
      FileOpenPicker openPicker = new FileOpenPicker();
      openPicker.ViewMode = PickerViewMode.Thumbnail;
      openPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
      openPicker.FileTypeFilter.Add(".avi");
      openPicker.FileTypeFilter.Add(".flv");

      StorageFile file = await openPicker.PickSingleFileAsync();
      var mediaItem = new MediaItem(file);

      this.AppContext.CurrentItem = mediaItem;
    }
  }
}

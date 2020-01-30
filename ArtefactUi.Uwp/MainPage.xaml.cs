using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Mt.MediaMan.ArtefactUi.Uwp
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page, INotifyPropertyChanged
  {
    private string _boundProp;

    public MainPage()
    {
      this.InitializeComponent();
      this._boundProp = "Initial value";

    }

    public string BoundPropSource
    {
      get
      {
        return this._boundProp;
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private async void HandleOpenClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
      var folderPicker = new Windows.Storage.Pickers.FolderPicker();
      folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
      folderPicker.FileTypeFilter.Add("*");
      //var folder = await folderPicker.PickSingleFolderAsync();


      this._boundProp = "Changed value";
      this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(BoundPropSource)));

    }
  }
}

using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ArtefactUi.Uwp
{
  public sealed partial class MainToolbarControl : UserControl
  {
    public MainToolbarControl()
    {
      this.InitializeComponent();
    }

    private async void HandleOpenCatalogClick(object sender, RoutedEventArgs e)
    {
      var messageDialog = new MessageDialog("Open dialog!");
      await messageDialog.ShowAsync();
    }
  }
}

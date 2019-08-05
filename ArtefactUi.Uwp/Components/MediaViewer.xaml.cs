using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Mt.MediaMan.ArtefactUi.Uwp.Components
{
  public sealed partial class MediaViewer : UserControl
  {
    public MediaViewer()
    {
      this.InitializeComponent();
    }

    private void HandleOpenClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
      int a = 4;
      a++;
    }
  }
}

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Mt.MediaMan.Artefactory.Uwp.Components
{
  public sealed partial class MainToolbar : UserControl
  {
    public MainToolbar()
    {
      this.InitializeComponent();
    }

    private void HandleOpenClick(object sender, RoutedEventArgs e)
    {
      int a = 4;
      a++;
    }
  }
}

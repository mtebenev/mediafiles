using Mt.MediaMan.ArtefactUi.Uwp.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Mt.MediaMan.ArtefactUi.Uwp.MediaViewer
{
  public sealed partial class ExplorerPane : UserControl
  {
    private static readonly DependencyProperty _dpMediaItem =
    DependencyProperty.Register(nameof(BoundProp), typeof(MediaItem), typeof(ViewerPane), new PropertyMetadata(0));

    public ExplorerPane()
    {
      this.InitializeComponent();
    }

    // MediaItem DP
    public static DependencyProperty BoundPropProperty => _dpMediaItem;
    public string BoundProp
    {
      get => (string)GetValue(BoundPropProperty);
      set
      {
        SetValue(BoundPropProperty, value);
      }
    }

  }
}

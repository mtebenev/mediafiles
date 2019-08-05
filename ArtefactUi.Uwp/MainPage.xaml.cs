using Mt.MediaMan.ArtefactUi.Uwp.Core;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ArtefactUi.Uwp
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    private readonly MmAppContext _appContext;

    public MainPage()
    {
      this.InitializeComponent();
      this._appContext = new MmAppContext();
    }

    public MmAppContext AppContext => _appContext;
  }
}

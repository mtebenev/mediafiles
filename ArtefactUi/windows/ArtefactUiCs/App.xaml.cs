using ArtefactUiUwpLib;
using Microsoft.ReactNative;

namespace ArtefactUiCs
{
  /// <summary>
  /// Provides application-specific behavior to supplement the default
  /// Application class.
  /// </summary>
  sealed partial class App : ReactApplication
  {
    /// <summary>
    /// Initializes the singleton application object.  This is the first line
    /// of authored code executed, and as such is the logical equivalent of
    /// main() or WinMain().
    /// </summary>
    public App()
    {
      MainComponentName = "ArtefactUi";

#if BUNDLE
            JavaScriptBundleFile = "index.windows";
            InstanceSettings.UseWebDebugger = false;
            InstanceSettings.UseLiveReload = false;
#else
      JavaScriptMainModuleName = "index";
      InstanceSettings.UseWebDebugger = true;
      InstanceSettings.UseLiveReload = true;
#endif

#if DEBUG
      InstanceSettings.EnableDeveloperMenu = true;
#else
            InstanceSettings.EnableDeveloperMenu = false;
#endif

      PackageProviders.Add(new Microsoft.ReactNative.Managed.ReactPackageProvider());
      PackageProviders.Add(new ArtefactUiUwpLib.ReactPackageProvider());

      InitializeComponent();
    }
  }
}

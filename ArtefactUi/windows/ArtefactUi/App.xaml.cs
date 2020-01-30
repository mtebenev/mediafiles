using Microsoft.ReactNative;

namespace ArtefactUi
{
  sealed partial class App : ReactApplication
  {
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

      PackageProviders.Add(new Microsoft.ReactNative.Managed.ReactPackageProvider()); // Includes any modules in this project
      PackageProviders.Add(new Mt.MediaMan.ArtefactUi.AppCoreLib.ReactPackageProvider());

      InitializeComponent();
    }
  }
}

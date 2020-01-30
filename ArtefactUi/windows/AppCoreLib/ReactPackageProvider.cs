using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;

namespace Mt.MediaMan.ArtefactUi.AppCoreLib
{
  public sealed class ReactPackageProvider : IReactPackageProvider
  {
    public void CreatePackage(IReactPackageBuilder packageBuilder)
    {
      packageBuilder.AddAttributedModules();
    }
  }
}

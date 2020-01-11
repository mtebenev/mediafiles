using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;

namespace ArtefactUiUwpLib
{
  public class ReactPackageProvider : IReactPackageProvider
  {
    public void CreatePackage(IReactPackageBuilder packageBuilder)
    {
      packageBuilder.AddAttributedModules();
      packageBuilder.AddViewManagers();
    }
  }
}

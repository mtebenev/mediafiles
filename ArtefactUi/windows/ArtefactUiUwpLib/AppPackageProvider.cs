using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;

namespace ExtLib
{
  public class AppPackageProvider : IReactPackageProvider
  {
    public void CreatePackage(IReactPackageBuilder packageBuilder)
    {
      packageBuilder.AddAttributedModules();
      packageBuilder.AddViewManagers();
    }
  }
}

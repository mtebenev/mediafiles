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

      //////////////////////////////////
      /*
      packageBuilder.AddModule("FsModule", (IReactModuleBuilder moduleBuilder) =>
      {
        var module = new FsModule();
        //moduleBuilder.SetName("FancyMath");
        moduleBuilder.AddMethod("getContents", MethodReturnType.Callback,
          (IJSValueReader inputReader,
          IJSValueWriter outputWriter,
          MethodResultCallback resolve,
          MethodResultCallback reject) =>
          {
            module.GetContents().ContinueWith(rr =>
            {
              outputWriter.WriteArrayBegin();
              outputWriter.WriteDouble(100);
              outputWriter.WriteArrayEnd();
              resolve(outputWriter);

            });
          });
        return module;
      });
    */
    }
  }
}

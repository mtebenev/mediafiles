using System.Collections.Generic;

namespace AppCoreLib.Rn
{
  public delegate void RnMethodDelegate(string args);
  public delegate void RnMethodCallback(IReadOnlyList<string> pCallBack);
  public delegate void RnMethodWithCallbackDelegate(string args, RnMethodCallback callBack);

  /// <summary>
  /// React native module interface.
  /// Basically the same as react.uwp.IModule. We need it here because react.uwp cannot be referenced from .Net standard library.
  /// </summary>
  public interface IRnModule
  {
    IReadOnlyDictionary<string, string> Constants { get; }
    IReadOnlyDictionary<string, RnMethodDelegate> Methods { get; }
    IReadOnlyDictionary<string, RnMethodWithCallbackDelegate> MethodsWithCallback { get; }
    string Name { get; }
  }
}

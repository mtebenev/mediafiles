using System;
using System.Collections.Generic;
using System.Linq;
using AppCoreLib.Rn;
using react.uwp;

namespace ArtefactUi.Core
{
  /// <summary>
  /// Wraps .Net module and provides it to RN app.
  /// </summary>
  internal class RnWrapperModule : react.uwp.IModule
  {
    /// <summary>
    /// Wrapped module;
    /// </summary>
    private IRnModule _rnModule;

    Dictionary<string, string> _constants;
    Dictionary<string, MethodDelegate> _delegates;
    Dictionary<String, MethodWithCallbackDelegate> _delegatesWithCallback;

    public RnWrapperModule(IRnModule rnModule)
    {
      _rnModule = rnModule;
      _constants = new Dictionary<string, string>(rnModule.Constants);
      _delegates = new Dictionary<string, MethodDelegate>();
      _delegatesWithCallback = new Dictionary<string, MethodWithCallbackDelegate>();

      // Methods
      foreach (var key in rnModule.Methods.Keys)
      {
        _delegates.Add(
          key,
          new MethodDelegate((string para) =>
          {
            _rnModule.Methods[key](para);
          }));
      }

      // Methods with callbacks
      foreach (var key in rnModule.MethodsWithCallback.Keys)
      {
        _delegatesWithCallback.Add(
          key,
          new MethodWithCallbackDelegate((string para, MethodCallback callback) =>
          {
            _rnModule.MethodsWithCallback[key](
              para, (target) =>
              {
                callback(target);
              });
          }));
      }
    }

    /// <summary>
    /// IModule
    /// </summary>
    public IReadOnlyDictionary<string, string> Constants => _constants;

    /// <summary>
    /// IModule
    /// </summary>
    public IReadOnlyDictionary<string, MethodDelegate> Methods => _delegates;

    /// <summary>
    /// IModule
    /// </summary>
    public IReadOnlyDictionary<string, MethodWithCallbackDelegate> MethodsWithCallback => _delegatesWithCallback;

    /// <summary>
    /// IModule
    /// </summary>
    public string Name => _rnModule.Name;
  }
}

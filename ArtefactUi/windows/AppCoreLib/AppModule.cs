using AppCoreLib.Rn;
using System;
using System.Collections.Generic;

namespace AppCoreLib
{
  /// <summary>
  /// Main application module
  /// </summary>
  public class AppModule : IRnModule
  {
    private Dictionary<string, string> _constants;
    private Dictionary<string, RnMethodDelegate> _delegates;
    private Dictionary<String, RnMethodWithCallbackDelegate> _delegatesWithCallback;

    public AppModule()
    {
      _constants = new Dictionary<string, string>();
      _delegates = new Dictionary<string, RnMethodDelegate>();
      _delegatesWithCallback = new Dictionary<string, RnMethodWithCallbackDelegate>();

      InitMethods();
    }

    /// <summary>
    /// IRnModule
    /// </summary>
    public IReadOnlyDictionary<string, string> Constants => _constants;

    /// <summary>
    /// IRnModule
    /// </summary>
    public IReadOnlyDictionary<string, RnMethodDelegate> Methods => _delegates;

    /// <summary>
    /// IRnModule
    /// </summary>
    public IReadOnlyDictionary<string, RnMethodWithCallbackDelegate> MethodsWithCallback => _delegatesWithCallback;

    /// <summary>
    /// IRnModule
    /// </summary>
    public string Name => "AppModule";

    private void InitMethods()
    {
      _delegatesWithCallback.Add("getItems", new RnMethodWithCallbackDelegate(
        (string para, RnMethodCallback callback) =>
        {
          int a = 4;
          a++;
          callback(new string[] { "[\"Item 1\", \"Item 2\", \"Item 3\"]" });
        }));
    }
  }
}

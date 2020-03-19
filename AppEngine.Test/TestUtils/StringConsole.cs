using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.AppEngine.Test.TestUtils
{
  /// <summary>
  /// Grabs all the output to the string.
  /// </summary>
  public class StringConsole : IConsole, IDisposable
  {
    private TextWriter _out;

    public StringConsole()
    {
      this._out = new StringWriter();
    }

    public TextWriter Out => this._out;

    public TextWriter Error => throw new NotImplementedException();

    public TextReader In => throw new NotImplementedException();

    public bool IsInputRedirected => throw new NotImplementedException();

    public bool IsOutputRedirected => throw new NotImplementedException();

    public bool IsErrorRedirected => throw new NotImplementedException();

    public ConsoleColor ForegroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public ConsoleColor BackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event ConsoleCancelEventHandler CancelKeyPress;

    public void Dispose()
    {
      this._out.Dispose();
    }

    public void ResetColor()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// The full console output.
    /// </summary>
    public string GetText()
    {
      return this._out.ToString();
    }
  }
}

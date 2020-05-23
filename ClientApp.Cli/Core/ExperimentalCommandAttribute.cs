using System;

namespace Mt.MediaFiles.ClientApp.Cli.Core
{
  /// <summary>
  /// The marker attribute for experimental commands.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  internal class ExperimentalCommandAttribute : Attribute
  {
  }
}

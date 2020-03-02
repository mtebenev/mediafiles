using System;
using System.IO;

namespace Mt.MediaMan.AppEngine.Common
{
  /// <summary>
  /// Enumerates parts of a file path.
  /// </summary>
  public class PathPartsEnumerator
  {
    public PathPartsEnumerator(string path)
    {
      if(String.IsNullOrEmpty(path))
      {
        throw new Exception("Invalid path");
      }

      this.Root = Path.GetPathRoot(path);
      this.Parts = path.Substring(this.Root.Length)
        .Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// The root part.
    /// </summary>
    public string Root { get; }

    /// <summary>
    /// The remaining parts
    /// </summary>
    public string[] Parts { get; }
  }
}

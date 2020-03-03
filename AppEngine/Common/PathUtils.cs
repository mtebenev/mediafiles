using System;
using System.IO;
using System.Linq;

namespace Mt.MediaMan.AppEngine.Common
{
  /// <summary>
  /// Various helpers for working with paths
  /// </summary>
  public static class PathUtils
  {
    /// <summary>
    /// Returns true if the path is a subdirectory of the base path.
    /// Note: more sophisticated solution: https://stackoverflow.com/questions/5617320/given-full-path-check-if-path-is-subdirectory-of-some-other-path-or-otherwise
    /// </summary>
    public static bool IsBaseOfPath(string path, string basePath)
    {
      var baseUri = new Uri(basePath);
      var uri = new Uri(path);

      var result = baseUri.IsBaseOf(uri);
      return result;
    }

    /// <summary>
    /// Collects the relative parts of the path to the base path.
    /// </summary>
    public static string[] GetRelativeParts(string path, string basePath)
    {
      if(String.IsNullOrEmpty(path) || String.IsNullOrEmpty(basePath))
      {
        throw new Exception("Invalid path");
      }

      var baseUri = new Uri(basePath);
      var uri = new Uri(path);
      var relativeUri = baseUri.MakeRelativeUri(uri);

      var parts = relativeUri
        .ToString()
        .Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries)
        .Skip(1)
        .ToArray();

      return parts;
    }

    /// <summary>
    /// Creates a relative path from one file or folder to another.
    /// TODO: Use Path.GetRelativePath when migrated to .Net Standard 2.1
    /// </summary>
    /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
    /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
    /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="UriFormatException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static string MakeRelativePath(string fromPath, string toPath)
    {
      if(String.IsNullOrEmpty(fromPath))
        throw new ArgumentNullException("fromPath");
      if(String.IsNullOrEmpty(toPath))
        throw new ArgumentNullException("toPath");

      Uri fromUri = new Uri(fromPath);
      Uri toUri = new Uri(toPath);

      if(fromUri.Scheme != toUri.Scheme)
      { return toPath; } // path can't be made relative.

      Uri relativeUri = fromUri.MakeRelativeUri(toUri);
      string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

      if(toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
      {
        relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
      }

      return relativePath;
    }
  }
}

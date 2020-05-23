using System;
using System.IO.Abstractions;
using System.Linq;

namespace Mt.MediaFiles.AppEngine.Video.Common
{
  /// <summary>
  /// Checks for common supported file extensions.
  /// </summary>
  internal static class FileExtensionCheck
  {
    private static readonly string[] SupportedExtensions = new[] { ".mp4", ".avi", ".mkv", ".flv", ".wmv", ".3gp" };

    /// <summary>
    /// Checks if the given path has a supported video extension.
    /// </summary>
    public static bool IsVideo(IFileSystem fileSystem, string path)
    {
      var extension = fileSystem.Path.GetExtension(path);
      return SupportedExtensions.Any(e => e.Equals(extension, StringComparison.OrdinalIgnoreCase));
    }
  }
}

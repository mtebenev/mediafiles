using System;

namespace Mt.MediaFiles.AppEngine.Common
{
  /// <summary>
  /// Various string helpers
  /// </summary>
  public static class StringUtils
  {
    public static string BytesToString(long byteCount)
    {
      string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
      if(byteCount == 0)
        return "0" + suf[0];
      var bytes = Math.Abs(byteCount);
      var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
      var num = Math.Round(bytes / Math.Pow(1024, place), 1);

      return Math.Sign(byteCount) * num + suf[place];
    }
  }
}

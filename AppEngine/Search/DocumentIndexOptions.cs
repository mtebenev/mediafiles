using System;

namespace Mt.MediaFiles.AppEngine.Search
{
  [Flags]
  public enum DocumentIndexOptions
  {
    None = 0,
    Store = 1,
    Analyze = 2,
    Sanitize = 4
  }
}

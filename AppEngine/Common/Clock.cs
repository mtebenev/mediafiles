using System;

namespace Mt.MediaFiles.AppEngine.Common
{
  public class Clock : IClock
  {
    public DateTime UtcNow => DateTime.UtcNow;
  }
}

using System;

namespace Mt.MediaMan.AppEngine.Common
{
  public class Clock : IClock
  {
    public DateTime UtcNow => DateTime.UtcNow;
  }
}

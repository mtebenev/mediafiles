using System;

namespace Mt.MediaFiles.AppEngine.FileStorage
{
  public class FileStoreException : Exception
  {
    public FileStoreException(string message) : base(message)
    {
    }

    public FileStoreException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}

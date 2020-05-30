using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Common
{
  /// <summary>
  /// Handy API for semaphore slim.
  /// </summary>
  public class AsyncSemaphoreLock
  {
    private readonly SemaphoreSlim _semaphore;

    public AsyncSemaphoreLock()
    {
      _semaphore = new SemaphoreSlim(1, 1);
    }

    public async Task<LockReleaser> Lock()
    {
      await this._semaphore.WaitAsync();
      return new LockReleaser(_semaphore);
    }

    public struct LockReleaser : IDisposable
    {
      private readonly SemaphoreSlim _semaphore;

      public LockReleaser(SemaphoreSlim semaphore)
      {
        this._semaphore = semaphore;
      }
      public void Dispose()
      {
        this._semaphore.Release();
      }
    }
  }
}

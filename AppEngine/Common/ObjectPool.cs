using System;
using System.Collections.Concurrent;

namespace Mt.MediaFiles.AppEngine.Common
{
  /// <summary>
  /// Simple object pool.
  /// Source: https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool
  /// </summary>
  public class ObjectPool<T>
  {
    private readonly ConcurrentBag<T> _objects;
    private readonly Func<T> _objectGenerator;

    public ObjectPool(Func<T> objectGenerator)
    {
      this._objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
      this._objects = new ConcurrentBag<T>();
    }

    public T Get() => _objects.TryTake(out T item) ? item : _objectGenerator();

    public void Return(T item) => _objects.Add(item);
  }
}

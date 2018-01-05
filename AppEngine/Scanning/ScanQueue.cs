using System.Collections.Generic;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ScanQueue : IScanQueue
  {
    private readonly Queue<IScanQueueEntry> _queue;

    public ScanQueue()
    {
      _queue = new Queue<IScanQueueEntry>();
    }

    public void Enqueue(IScanQueueEntry scanQueueEntry)
    {
      _queue.Enqueue(scanQueueEntry);
    }

    public IScanQueueEntry Dequeue()
    {
      return _queue.Dequeue();
    }

    public int Count => _queue.Count;
  }
}

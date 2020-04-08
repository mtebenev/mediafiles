namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Encapsulates scan queue logic
  /// </summary>
  internal interface IScanQueue
  {
    void Enqueue(IScanQueueEntry scanQueueEntry);
    IScanQueueEntry Dequeue();

    int Count { get; }
  }
}

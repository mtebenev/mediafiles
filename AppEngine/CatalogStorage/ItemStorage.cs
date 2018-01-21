using System;
using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  internal class ItemStorage : IItemStorage
  {
    private int _nextItemId;

    public ItemStorage()
    {
      _nextItemId = 1;
    }

    public Task<int> CreateItem(CatalogItemRecord itemRecord)
    {
      Console.WriteLine($"Added item: {itemRecord.Name}");
      return Task.FromResult(_nextItemId++);
    }
  }
}

using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Encapsulates output match group.
  /// </summary>
  public class MatchOutputGroup
  {
    private readonly List<MatchOutputItem> _items;

    /// <summary>
    /// Ctor.
    /// </summary>
    public MatchOutputGroup(string baseItem)
    {
      this.BaseItem = baseItem;
      this._items = new List<MatchOutputItem>();
    }

    public string BaseItem { get; }

    /// <summary>
    /// The group items.
    /// </summary>
    public IReadOnlyList<MatchOutputItem> Items => this._items;

    public void AddItem(MatchOutputItem item)
    {
      this._items.Add(item);
    }
  }
}

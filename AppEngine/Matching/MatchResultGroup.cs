using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// The match group in search result.
  /// </summary>
  public class MatchResultGroup
  {
    private readonly List<int> _itemIds;

    public MatchResultGroup(int baseItemId)
    {
      this.BaseItemId = baseItemId;
      this._itemIds = new List<int>();
    }

    /// <summary>
    /// The base item.
    /// </summary>
    public int BaseItemId { get; }

    /// <summary>
    /// Other items in the group.
    /// </summary>
    public IReadOnlyList<int> ItemIds => this._itemIds;

    /// <summary>
    /// Adds another item ID.
    /// </summary>
    public void AddItemId(int itemId)
    {
      this._itemIds.Add(itemId);
    }
  }
}

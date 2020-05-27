using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Encapsulates information about an item in output.
  /// </summary>
  public class MatchOutputItem
  {
    private readonly List<MatchOutputProperty> _properties;

    /// <summary>
    /// Ctor.
    /// </summary>
    public MatchOutputItem(string item)
    {
      this.Item = item;
      this._properties = new List<MatchOutputProperty>();
    }

    /// <summary>
    /// The item title.
    /// </summary>
    public string Item { get; }

    /// <summary>
    /// The comparison properties.
    /// </summary>
    public IReadOnlyList<MatchOutputProperty> Properties => this._properties;

    /// <summary>
    /// Adds another property to the item.
    /// </summary>
    public void AddProperty(MatchOutputProperty property)
    {
      this._properties.Add(property);
    }
  }
}

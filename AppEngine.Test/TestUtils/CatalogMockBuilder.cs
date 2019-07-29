using Mt.MediaMan.AppEngine.Cataloging;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System.Linq;

namespace Mt.MediaMan.AppEngine.Test.TestUtils
{
  public class CatalogMockBuilder
  {
    /// <summary>
    /// Standard mock catalog layout
    /// </summary>
    private static readonly string CatalogDefStd = @"
{
  name: 'Root',
  children: [
    {
      name: 'Folder 1',
      children: [
        {name: 'Item 1.1'},
        {name: 'Item 1.2'},
        {name: 'Item 1.3'},
        {
          name: 'Folder 1.1',
          children: [
            {name: 'Item 1.1.1'},
            {name: 'Item 1.1.2'},
            {name: 'Item 1.1.3'},
          ]
        }
      ]
    },
    {
      name: 'Folder 2',
      children: [
        {name: 'Item 2.1'},
        {name: 'Item 2.2'},
        {name: 'Item 2.3'},
        {
          name: 'Folder 2.1',
          children: [
            {name: 'Item 2.1.1'},
            {name: 'Item 2.1.2'},
            {name: 'Item 2.1.3'},
          ]
        }
      ]
    },
    {
      name: 'Folder 3',
      children: [
        {name: 'Item 3.1'},
        {name: 'Item 3.2'},
        {name: 'Item 3.3'},
        {
          name: 'Folder 3.1',
          children: [
            {name: 'Item 3.1.1'},
            {name: 'Item 3.1.2'},
            {name: 'Item 3.1.3'},
          ]
        }
      ]
    },
  ]
}
";
    private int _nextItemId;
    private string _catalogDef;

    public CatalogMockBuilder(string catalogDef)
    {
      this._nextItemId = 1;
      this._catalogDef = catalogDef;
    }

    /// <summary>
    /// Factory method: creates catalog mock with default structure.
    /// </summary>
    public static CatalogMockBuilder CreateDefault()
    {
      var result = CatalogMockBuilder.Create(CatalogDefStd);
      return result;
    }

    /// <summary>
    /// Factory method: creates catalog mock with given definition.
    /// </summary>
    public static CatalogMockBuilder Create(string rootItemDef)
    {
      var builder = new CatalogMockBuilder(rootItemDef);
      return builder;
    }

    public ICatalog Build()
    {
      var mockCatalog = Substitute.For<ICatalog>();
      var catalogDefObject = JObject.Parse(this._catalogDef);
      this.DeserializeItemDef(mockCatalog, catalogDefObject);

      return mockCatalog;
    }

    private ICatalogItem DeserializeItemDef(ICatalog mockCatalog, JObject itemDef)
    {
      var mockCatalogItem = Substitute.For<ICatalogItem>();
      var itemId = this._nextItemId++;
      mockCatalogItem.CatalogItemId.Returns(itemId);
      mockCatalogItem.Name.Returns((string)itemDef["name"]);

      mockCatalog.GetItemByIdAsync(itemId).Returns(mockCatalogItem);

      if (itemDef["children"] != null)
      {
        var children = itemDef["children"]
        .Select(c => this.DeserializeItemDef(mockCatalog, (JObject)c))
        .ToList();
        mockCatalogItem.GetChildrenAsync().Returns(children);
      }

      return mockCatalogItem;
    }
  }
}

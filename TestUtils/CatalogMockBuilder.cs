using System;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.TestUtils
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

    /// <summary>
    /// Item name -> info part.
    /// </summary>
    private Dictionary<string, InfoPartVideo> _infoPartsVideo;

    public CatalogMockBuilder(string catalogDef)
    {
      this._nextItemId = 1;
      this._catalogDef = catalogDef;
      this._infoPartsVideo = new Dictionary<string, InfoPartVideo>();
    }

    /// <summary>
    /// Factory method: creates the catalog mock with default structure.
    /// </summary>
    public static CatalogMockBuilder CreateDefault()
    {
      var result = Create(CatalogDefStd);
      return result;
    }

    /// <summary>
    /// Factory method: creates the catalog mock with given definition.
    /// </summary>
    public static CatalogMockBuilder Create(string rootItemDef)
    {
      var builder = new CatalogMockBuilder(rootItemDef);
      return builder;
    }

    /// <summary>
    /// Add an info part to item with given name.
    /// </summary>
    public CatalogMockBuilder WithInfoPartVideo(string itemName, InfoPartVideo infoPart)
    {
      this._infoPartsVideo[itemName] = infoPart;
      return this;
    }

    /// <summary>
    /// Final call.
    /// </summary>
    public ICatalog Build()
    {
      var mockCatalog = Substitute.For<ICatalog>();
      var catalogDefObject = JObject.Parse(this._catalogDef);
      var rootItem = this.DeserializeItemDef(mockCatalog, catalogDefObject);
      mockCatalog.RootItem.Returns(rootItem);

      return mockCatalog;
    }

    private ICatalogItem DeserializeItemDef(ICatalog mockCatalog, JObject itemDef)
    {
      var mockCatalogItem = Substitute.For<ICatalogItem>();

      // Item ID
      var itemId = itemDef.GetValue("id") != null
        ? (int)itemDef["id"]
        : this._nextItemId++;
      mockCatalogItem.CatalogItemId.Returns(itemId);

      // Item name
      var itemPath = (string)itemDef["path"];
      mockCatalogItem.Path.Returns(itemPath);

      // Scan root info part
      var rootPath = (string)itemDef["rootPath"];
      if(!string.IsNullOrEmpty(rootPath))
      {
        var infoPartScanRoot = new InfoPartScanRoot
        {
          RootPath = rootPath
        };
        mockCatalogItem.GetInfoPartAsync<InfoPartScanRoot>().Returns(infoPartScanRoot);
      }

      if(itemDef["fileSize"] != null)
      {
        mockCatalogItem.Size.Returns((int)itemDef["fileSize"]);
        mockCatalogItem.ItemType.Returns(CatalogItemType.File);
        mockCatalogItem.Name.Returns(System.IO.Path.GetFileName(itemPath));
      }
      else
      {
        mockCatalogItem.Name.Returns(itemPath);
      }

      // Info parts
      this.BuildItemInfoParts(mockCatalogItem, this._infoPartsVideo);

      mockCatalog.GetItemByIdAsync(itemId).Returns(mockCatalogItem);
      this.DeserializeChildren(itemDef, mockCatalog, mockCatalogItem);

      return mockCatalogItem;
    }

    /// <summary>
    /// Prepares mock children for the item.
    /// </summary>
    private void DeserializeChildren(JObject itemDef, ICatalog mockCatalog, ICatalogItem mockCatalogItem)
    {
      if(itemDef["children"] != null)
      {
        var children = itemDef["children"]
        .Select(c => this.DeserializeItemDef(mockCatalog, (JObject)c))
        .ToList();

        mockCatalogItem.GetChildrenAsync().Returns(children);
        mockCatalogItem.IsDirectory.Returns(true);
      }
    }

    private void BuildItemInfoParts<TInfoPart>(ICatalogItem catalogItem, Dictionary<string, TInfoPart> infoParts) where TInfoPart : InfoPartBase
    {
      TInfoPart ip;
      if(infoParts.TryGetValue(catalogItem.Path, out ip))
      {
        catalogItem.GetInfoPartAsync<TInfoPart>().Returns(ip);
      }
    }
  }
}

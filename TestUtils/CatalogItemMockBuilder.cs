using System.Collections.Generic;
using System.Linq;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;

namespace Mt.MediaMan.AppEngine.Test.TestUtils
{
  public class CatalogItemMockBuilder
  {
    private ICatalogItem _mockCi;
    private IList<InfoPartBase> _infoParts;

    public CatalogItemMockBuilder()
    {
      this._mockCi = Substitute.For<ICatalogItem>();
      this._infoParts = new List<InfoPartBase>();
    }

    /// <summary>
    /// Factory method.
    /// </summary>
    public static CatalogItemMockBuilder Create()
    {
      var builder = new CatalogItemMockBuilder();
      return builder;
    }

    /// <summary>
    /// Add an info part to item with given name.
    /// </summary>
    public CatalogItemMockBuilder WithInfoPartVideo(InfoPartVideo infoPart)
    {
      this._mockCi.GetInfoPartAsync<InfoPartVideo>().Returns(infoPart);
      this._infoParts.Add(infoPart);
      return this;
    }

    /// <summary>
    /// Final call.
    /// </summary>
    public ICatalogItem Build()
    {
      var infoPartNames = this._infoParts
        .Select(x => x.GetType().Name)
        .ToList();
      this._mockCi.GetInfoPartNamesAsync().Returns(infoPartNames);

      return this._mockCi;
    }

  }
}

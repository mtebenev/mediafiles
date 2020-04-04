using System.Collections.Generic;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Common;

namespace Mt.MediaMan.ClientApp.Cli
{
  internal static class ShellConsoleUtils
  {
    public static void PrintItemsTable(IConsole console, IList<ICatalogItem> catalogItems)
    {
      TableBuilder tb = new TableBuilder();
      tb.AddRow("ID", "Name", "Size");
      tb.AddRow("--", "----", "----");

      foreach (var catalogItem in catalogItems)
        tb.AddRow(catalogItem.CatalogItemId, catalogItem.Name, StringUtils.BytesToString(catalogItem.Size));

      console.Write(tb.Output());
    }
  }
}

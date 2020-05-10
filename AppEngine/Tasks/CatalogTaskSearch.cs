using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.QueryParsers.Simple;
using Lucene.Net.Search;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Search;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  /// <summary>
  /// The task searches for items in the catalog.
  /// TODO MTE: check how orchard performs search, it uses MultiFieldQueryParser
  /// </summary>
  public sealed class CatalogTaskSearch : IInternalCatalogTask<IList<int>>
  {
    private readonly string _query;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogTaskSearch(string query)
    {
      this._query = query;
    }

    public Task<IList<int>> ExecuteAsync(ICatalog catalog)
    {
      return catalog.ExecuteTaskAsync(this);
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    async Task<IList<int>> IInternalCatalogTask<IList<int>>.ExecuteAsync(Catalog catalog)
    {
      var catalogItemIdStrings = new List<string>();
      var idSet = new HashSet<string>(new[] { "CatalogItemId" });

      // TODO MTE: it works only for file names, need to check other analyzers
      var analyzer = new StandardAnalyzer(SearchConstants.LuceneVersion);
      var queryParser = new SimpleQueryParser(analyzer, "Book.Title");

      var escapedQuery = QueryParserBase.Escape(this._query);
      var luceneQuery = queryParser.Parse(escapedQuery);

      await catalog.IndexManager.SearchAsync("default", searcher =>
      {
        // Fetch one more result than PageSize to generate "More" links
        var collector = TopScoreDocCollector.Create(100, true);

        searcher.Search(luceneQuery, collector);
        var hits = collector.GetTopDocs(0);

        foreach(var hit in hits.ScoreDocs)
        {
          var d = searcher.Doc(hit.Doc);
          catalogItemIdStrings.Add(d.GetField("CatalogItemId").GetStringValue());
        }

        return Task.CompletedTask;
      });

      var result = catalogItemIdStrings
        .Select(idString => int.Parse(idString))
        .ToList();

      return result;
    }
  }
}

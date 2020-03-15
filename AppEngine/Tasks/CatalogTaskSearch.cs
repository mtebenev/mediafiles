using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.QueryParsers.Simple;
using Lucene.Net.Search;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Search;


namespace Mt.MediaMan.AppEngine.Tasks
{
  /// <summary>
  /// The task searches for items in the catalog.
  /// TODO MTE: check how orchard performs search, it uses MultiFieldQueryParser
  /// </summary>
  public class CatalogTaskSearch : ICatalogTask
  {
    private readonly string _query;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogTaskSearch(string query)
    {
      this._query = query;
    }

    /// <summary>
    /// ICatalogTask.
    /// </summary>
    async Task ICatalogTask.ExecuteAsync(Catalog catalog)
    {
      var catalogItemIdStrings = new List<string>();
      var idSet = new HashSet<string>(new[] { "CatalogItemId" });

      // TODO MTE: it works only for file names, need to check other analyzers
      var analyzer = new StandardAnalyzer(SearchConstants.LuceneVersion);
      var queryParser = new SimpleQueryParser(analyzer, "Book.Title");

      string escapedQuery = QueryParser.Escape(this._query);
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
    }
  }
}

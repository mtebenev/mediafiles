using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Ebooks.Commands;
using Mt.MediaMan.AppEngine.Ebooks.Storage;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Searches for books in catalog
  /// </summary>
  [Command("search-book", Description = "Searches for books in catalog")]
  internal class ShellCommandSearchBook : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandSearchBook(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    [Argument(0, "query")]
    public string Query { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      if(!string.IsNullOrWhiteSpace(Query))
        throw new NotImplementedException();

      var command = new CommandSearchBook();
      var items = await command.ExecuteAsync(_executionContext.Catalog);

      foreach(EbookDocument document in items)
      {
        _shellAppContext.Console.WriteLine(document.Title);
      }

      return 0;
    }
  }
}

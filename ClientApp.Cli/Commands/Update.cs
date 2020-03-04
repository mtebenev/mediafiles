using System.IO.Abstractions;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Walks through the files starting from the current directory and updates the items.
  /// For now used to retrieve video imprints
  /// </summary>
  [Command("update", Description = "Updates information about files starting from the current directory")]
  internal class Update
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;
    private readonly IFileSystem _fileSystem;
    private readonly IVideoImprintStorage _videoImprintStorage;

    public Update(ICommandExecutionContext executionContext, ShellAppContext shellAppContext, IFileSystem fileSystem, IVideoImprintStorage videoImprintStorage)
    {
      this._executionContext = executionContext;
      this._shellAppContext = shellAppContext;
      this._fileSystem = fileSystem;
      this._videoImprintStorage = videoImprintStorage;
    }

    public async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
    {
      //var p = @"\\192.168.1.52\media_store_22\siterips";
      var p = @"C:\_films";

      var command = new CommandUpdate(this._fileSystem, this._videoImprintStorage);
      await command.ExecuteAsync(this._executionContext, p);

      return Program.CommandExitResult;
    }
  }
}

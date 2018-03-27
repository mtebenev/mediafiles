using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  internal class Program
  {
    private static IServiceProvider _services;
    private static ShellContext _shellContext;

    public static async Task<int> Main(string[] args)
    {
      var executionContext = await CreateExecutionContext();
      _shellContext = new ShellContext(executionContext.Catalog.RootItem);

      // Init service container
      _services = new ServiceCollection()
        .AddSingleton(executionContext)
        .AddSingleton(PhysicalConsole.Singleton)
        .AddSingleton(_shellContext)
        .BuildServiceProvider();

      int commandResult = 0;

      // Do execution
      do
      {
        var prompt = CreatePrompt(_shellContext.CurrentItem);
        var commandInput = Prompt.GetString(prompt, promptColor: ConsoleColor.DarkBlue);

        if(!String.IsNullOrEmpty(commandInput))
        {
          var commandArgs = commandInput.Split(' ');

          // Create Shell app
          var shellApp = new CommandLineApplication<Shell>();
          shellApp.Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(_services);

          commandResult = shellApp.Execute(commandArgs);
        }
      } while(commandResult != CommandExitResult);

      return 1;
    }

    public const int CommandExitResult = -1;

    /// <summary>
    /// Creates string prompt for CLI
    /// </summary>
    private static string CreatePrompt(ICatalogItem currentItem)
    {
      var result = $"{currentItem.Name}>";
      return result;
    }

    private static async Task<ICommandExecutionContext> CreateExecutionContext()
    {
      // Open catalog
      var connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=mediaman;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
      var catalog = Catalog.CreateCatalog(connectionString);
      await catalog.OpenAsync();

      // Init execution context
      var progressIndicator = new ProgressIndicatorConsole();
      ICommandExecutionContext executionContext = new CommandExecutionContext(catalog, progressIndicator);

      return executionContext;
    }
  }
}

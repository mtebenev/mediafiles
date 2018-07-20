using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Ebooks;

namespace Mt.MediaMan.ClientApp.Cli
{
  internal class Program
  {
    private static IServiceProvider _services;
    private static ShellAppContext _shellAppContext;

    public static async Task<int> Main(string[] args)
    {
      Console.SetWindowSize(220, 54);

      var executionContext = await CreateExecutionContext();
      _shellAppContext = new ShellAppContext(executionContext.Catalog.RootItem);

      // Init service container
      _services = new ServiceCollection()
        .AddSingleton(executionContext)
        .AddSingleton(PhysicalConsole.Singleton)
        .AddSingleton(_shellAppContext)
        .BuildServiceProvider();

      int commandResult = 0;

      // Do execution
      do
      {
        var prompt = CreatePrompt(_shellAppContext.CurrentItem);
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
      var storageConfiguration = new StorageConfiguration();
      EbooksModule.CreateStorageConfiguration(storageConfiguration);

      // Open catalog
      var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=mediaman;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
      var catalog = Catalog.CreateCatalog(connectionString);
      await catalog.OpenAsync(storageConfiguration);

      // Init execution context
      var progressIndicator = new ProgressIndicatorConsole();
      ICommandExecutionContext executionContext = new CommandExecutionContext(catalog, progressIndicator);

      return executionContext;
    }
  }
}

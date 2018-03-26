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
    private readonly ICommandExecutionContext _executionContext;
    private static IServiceProvider _services;

    public static async Task<int> Main(string[] args)
    {
      var executionContext = await CreateExecutionContext();

      // Init service container
      _services = new ServiceCollection()
        .AddSingleton(executionContext)
        .AddSingleton(PhysicalConsole.Singleton)
        .BuildServiceProvider();

      var app = new CommandLineApplication<Program>();

      app.Conventions
        .UseDefaultConventions()
        .UseConstructorInjection(_services);

      return app.Execute(args);
    }

    public const int CommandExitResult = -1;

    public Program(ICommandExecutionContext executionContext)
    {
      _executionContext = executionContext;
    }

    public int OnExecute()
    {
      int commandResult = 0;

      // Create Shell app
      var shellApp = new CommandLineApplication<Shell>();
      shellApp.Conventions
        .UseDefaultConventions()
        .UseConstructorInjection(_services);

      // Do execution
      do
      {
        var commandInput = Prompt.GetString(">", promptColor: ConsoleColor.DarkBlue);

        if(!String.IsNullOrEmpty(commandInput))
        {
          var commandArgs = commandInput.Split(' ');
          commandResult = shellApp.Execute(commandArgs);
        }
      } while(commandResult != CommandExitResult);

      return 1;
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

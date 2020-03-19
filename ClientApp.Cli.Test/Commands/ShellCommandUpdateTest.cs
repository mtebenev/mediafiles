using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Tasks;
using Mt.MediaMan.AppEngine.Video.Tasks;
using Mt.MediaMan.ClientApp.Cli;
using Mt.MediaMan.ClientApp.Cli.Commands;
using NSubstitute;
using Xunit;

namespace ClientApp.Cli.Test.Commands
{
  internal class ShellCommandUpdateTesing : ShellCommandUpdate
  {
    public ShellCommandUpdateTesing(IShellAppContext shellAppContext, ICatalogTaskUpdateVideoImprintsFactory updateVideoImprintsFactory)
      : base(shellAppContext, updateVideoImprintsFactory)
    {
    }

    public Task ExecuteAsync()
    {
      return this.OnExecuteAsync();
    }
  }

  public class ShellCommandUpdateTest
  {
    [Fact]
    public async Task Start_From_Current_Location()
    {
      var mockCurrentItem = Substitute.For<ICatalogItem>();
      var mockShellAppContext = Substitute.For<IShellAppContext>();
      mockShellAppContext.CurrentItem.Returns(mockCurrentItem);

      var mockFactory = Substitute.For<ICatalogTaskUpdateVideoImprintsFactory>();
      var mockTask = Substitute.For<CatalogTaskBase>();
      mockFactory.Create(mockCurrentItem).Returns(mockTask);

      var command = new ShellCommandUpdateTesing(mockShellAppContext, mockFactory);
      await command.ExecuteAsync();

      // Verify
      mockFactory.Received().Create(mockCurrentItem);
      await mockShellAppContext.Catalog.Received().ExecuteTaskAsync(mockTask);
    }
  }
}

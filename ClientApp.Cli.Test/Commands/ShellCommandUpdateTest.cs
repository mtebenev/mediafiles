using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli;
using Mt.MediaMan.ClientApp.Cli.Commands;
using NSubstitute;
using Xunit;

namespace ClientApp.Cli.Test.Commands
{
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

      var command = new ShellCommandUpdate(mockShellAppContext, mockFactory);
      await command.OnExecuteAsync();

      // Verify
      mockFactory.Received().Create(mockCurrentItem);
      await mockShellAppContext.Catalog.Received().ExecuteTaskAsync(mockTask);
    }
  }
}

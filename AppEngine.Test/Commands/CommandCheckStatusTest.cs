using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using NSubstitute;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Commands
{
  public class CommandCheckStatusTest
  {
    [Fact]
    public async Task Should_Compare_Files()
    {
      var mockCatalog = Substitute.For<ICatalog>();

      var command = new CommandCheckStatus();
      var result = await command.ExecuteAsync(mockCatalog, "dir1");
    }
  }
}

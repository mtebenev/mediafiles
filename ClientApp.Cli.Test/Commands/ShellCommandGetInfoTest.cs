using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using Mt.MediaMan.ClientApp.Cli;
using Mt.MediaMan.ClientApp.Cli.Commands;
using NSubstitute;
using Xunit;

namespace ClientApp.Cli.Test.Commands
{
  internal class ShellCommandGetInfoTesing : ShellCommandGetInfo
  {
    public ShellCommandGetInfoTesing(IShellAppContext shellAppContext)
      : base(shellAppContext)
    {
    }

    public Task ExecuteAsync()
    {
      return this.OnExecuteAsync();
    }
  }

  public class ShellCommandGetInfoTest
  {
    [Fact]
    public async Task Print_Video_Info()
    {
      var mockCi = CatalogItemMockBuilder
        .Create()
        .WithInfoPartVideo(new InfoPartVideo
        {
          Duration = 1,
          Title = "some_title",
          VideoCodecLongName = "codec_name",
          VideoHeight = 200,
          VideoWidth = 300
        })
        .Build();

      var console = new StringConsole();
      var mockShellAppContext = Substitute.For<IShellAppContext>();
      mockShellAppContext.Console.Returns(console);
      mockShellAppContext.Catalog.GetItemByIdAsync(2).Returns(mockCi);

      var command = new ShellCommandGetInfoTesing(mockShellAppContext);
      command.ItemNameOrId = ":2";

      await command.ExecuteAsync();

      var output = console.GetText();
      output.Should().ContainEquivalentOf(
        "Title: some_title",
        "Duration: 00:00:01",
        "Width: 300",
        "Height: 200",
        "Codec: codec_name"
        );

      console.Dispose();
    }
  }
}

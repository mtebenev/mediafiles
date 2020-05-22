using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.ClientApp.Cli.Commands.Shell;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Commands
{
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

      var command = new ShellCommandGetInfo(mockShellAppContext);
      command.ItemNameOrId = ":2";

      var result = await command.OnExecuteAsync();

      var output = console.GetText();
      output.Should().ContainEquivalentOf(
        "Title: some_title",
        "Duration: 00:00:01",
        "Width: 300",
        "Height: 200",
        "Codec: codec_name"
        );

      console.Dispose();
      Assert.Equal(Program.CommandResultContinue, result);
    }
  }
}

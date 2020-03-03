using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Commands
{
  public class CommandCheckStatusTest
  {
    [Fact]
    public async Task Should_Compare_Files()
    {
      var catalogDef = @"
{
  name: 'Root',
  children: [
    {
      name: 'root_folder',
      rootPath: 'x:\\root_folder',
      children: [
        {
          name: 'folder1',
          children: [
            {
              name: 'folder2',
              children: [
                {
                  name: 'file1.txt'
                },
                {
                  name: 'file2.txt'
                },
              ]
            }
          ]
        }
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();

      var command = new CommandCheckStatus();
      var result = await command.ExecuteAsync(mockCatalog, @"x:\root_folder\folder1");
    }
  }
}

using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.ClientApp.Cli.Core;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Test
{
  public class PathArgumentResolverTest
  {
    [Fact]
    public void Resolve_Current_Directory_Files()
    {
      var mockFs = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
          { @"x:\folder1\file1.txt", new MockFileData("abc") },
          { @"x:\folder1\file2.txt", new MockFileData("abc") },
        },
        @"x:\folder1"
      );
      var mockCatalogSettings = Substitute.For<ICatalogSettings>();

      var resolver = new PathArgumentResolver(mockFs);
      var result = resolver.ResolveMany(null, mockCatalogSettings);

      Assert.Equal(
        new[] {
          @"x:\folder1\file1.txt",
          @"x:\folder1\file2.txt"
        },
        result);
    }
  }
}

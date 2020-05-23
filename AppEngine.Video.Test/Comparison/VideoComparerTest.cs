using System.Linq;
using AppEngine.Video.Comparison;
using Xunit;

namespace AppEngine.Video.Test.Comparison
{
  public class VideoComparerTest
  {
    [Fact]
    public void Compare_Identical_Imprints()
    {
      var data1 = Enumerable.Range(0, 100).Select(x => (byte)100).ToArray();
      var data2 = Enumerable.Range(0, 100).Select(x => (byte)100).ToArray();

      var comparer = new VideoImprintComparer();
      var result = comparer.Compare(data1, data2);

      Assert.True(result);
    }

    [Fact]
    public void Compare_Different_Imprints()
    {
      var data1 = Enumerable.Range(0, 100).Select(x => (byte)0).ToArray();
      var data2 = Enumerable.Range(0, 100).Select(x => (byte)255).ToArray();

      var comparer = new VideoImprintComparer();
      var result = comparer.Compare(data1, data2);

      Assert.False(result);
    }
  }
}

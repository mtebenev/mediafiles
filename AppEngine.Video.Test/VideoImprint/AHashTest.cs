using System.Collections;
using System.Linq;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.VideoImprint
{
  public class AHashTest
  {
    [Fact]
    public void Create_Hash()
    {
      var thumbBytes = new byte[64];
      for(int i = 0; i < thumbBytes.Length; i++)
      {
        thumbBytes[i] = i % 2 == 0
          ? (byte)0
          : (byte)255;
      }

      var ahash = new AHash(8);
      var resultHash = ahash.ComputeHash(thumbBytes);

      // Verify
      var bitArray = new BitArray(resultHash);
      var bits = bitArray.Cast<bool>().ToList();
      Assert.True(bits.Where((b, i) => i % 2 == 0).All(b => b == false));
      Assert.True(bits.Where((b, i) => i % 2 != 0).All(b => b == true));
    }

    [Fact]
    public void Similarity_1()
    {
      var thumb1 = new byte[64];
      var thumb2 = new byte[64];
      thumb2[0] = 255;

      var ahash = new AHash(8);
      var hash1 = ahash.ComputeHash(thumb1);
      var hash2 = ahash.ComputeHash(thumb2);

      var result = ahash.ComputeSimilarity(hash1, hash2);

      Assert.True(result > 90 && result < 100);
    }
  }
}

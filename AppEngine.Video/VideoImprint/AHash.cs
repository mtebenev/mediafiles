using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Mt.MediaFiles.AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The AHash implementation.
  /// (the canonical work is: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html).
  /// Design note: this class is not thread-safe (and computation is not re-entrant).
  /// </summary>
  internal class AHash
  {
    private readonly int _dimensionSize;
    private readonly int _pixelCount;
    private readonly BitArray _hashBits;
    private readonly int _hashSize; // The hash size in bytes.

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dimensionSize">We are computing the ahash of dimensionSizexdimensionSize. The dimension size essentially defines the precision of the hash value.</param>
    public AHash(int dimensionSize)
    {
      if(dimensionSize < 8 || dimensionSize % 8 != 0)
      {
        // For simplicity let's work with 8-multple dimensions: 8, 16, 24, 32 etc.
        throw new InvalidOperationException("The dimension size for hash should bу a multiple of 8.");
      }

      this._dimensionSize = dimensionSize;
      this._pixelCount = dimensionSize * dimensionSize;
      this._hashBits = new BitArray(this._pixelCount, false);
      this._hashSize = this._pixelCount / 8;
    }

    /// <summary>
    /// Computes the ahash.
    /// </summary>
    /// <param name="thumbnailData">8bit graycolor values.</param>
    public byte[] ComputeHash(byte[] thumbnailData)
    {
      var result = new byte[this._hashSize];
      this._hashBits.SetAll(false);

      // Calc average
      int average = 0;
      for(int i = 0; i < thumbnailData.Length; i++)
      {
        average += thumbnailData[i];
      }
      average /= thumbnailData.Length;

      // Create the hash (1 for higher than the average)
      // Note: the result hash value is 'reversed' but that's not important because all the hashes created
      // using the same procedure.
      for(int i = 0; i < this._pixelCount; i++)
      {
        if(thumbnailData[i] > average)
          this._hashBits.Set(i, true);
      }

      this._hashBits.CopyTo(result, 0);

      return result;
    }

    /// <summary>
    /// Given two a-hash values, compute their similarity (in percents).
    /// The spectacular discussion is at: https://stackoverflow.com/questions/109023/how-to-count-the-number-of-set-bits-in-a-32-bit-integer
    /// </summary>
    public double ComputeSimilarity(byte[] hash1, byte[] hash2)
    {
      // Calc the hamming weight for the bits by dwords
      var bitCount = 0u;
      UInt32 hashChunk, c1, c2;
      for(int i = 0; i < this._hashSize; i+=4)
      {
        c1 = MemoryMarshal.Cast<byte, UInt32>(hash1.AsSpan(i, 4))[0];
        c2 = MemoryMarshal.Cast<byte, UInt32>(hash2.AsSpan(i, 4))[0];
        hashChunk = c1 ^ c2;
        hashChunk = hashChunk - ((hashChunk >> 1) & 0x55555555);
        hashChunk = (hashChunk & 0x33333333) + ((hashChunk >> 2) & 0x33333333);
        var chunkCount = (((hashChunk + (hashChunk >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        bitCount += chunkCount;
      }

      var result = (this._pixelCount - bitCount) * 100 / (double)this._pixelCount;
      return result;
    }
  }
}

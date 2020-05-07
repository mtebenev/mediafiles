namespace AppEngine.Video.Comparison
{
  /// <summary>
  /// Mockable interface for the comparison.
  /// </summary>
  internal interface IVideoImprintComparer
  {
    /// <summary>
    /// Compares data of two video imprints.
    /// Returns true if the two videos are identical.
    /// </summary>
    bool Compare(byte[] videoImprintData1, byte[] videoImprintData2);
  }

  public interface IVideoImprintComparerFactory
  {
    internal IVideoImprintComparer Create();
  }
}

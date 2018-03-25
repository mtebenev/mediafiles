namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Thin UI callback interface
  /// </summary>
  public interface IProgressIndicator
  {
    /// <summary>
    /// Use to update UI with ongoing status
    /// </summary>
    void WriteStatus(string status);
  }
}

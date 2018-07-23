namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Thin UI callback interface
  /// </summary>
  public interface IProgressIndicator
  {
    /// <summary>
    /// Use to start a UI operation. status can be null
    /// </summary>
    IProgressOperation StartOperation(string status);
  }
}

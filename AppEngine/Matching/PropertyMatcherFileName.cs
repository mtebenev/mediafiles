using System;
using System.IO;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Matches file names of two items.
  /// </summary>
  public class PropertyMatcherFileName : IPropertyMatcher
  {
    private readonly IInfoPartAccess _baseItemAcess;
    private readonly IInfoPartAccess _otherItemAccess;

    /// <summary>
    /// Ctor.
    /// </summary>
    public PropertyMatcherFileName(IInfoPartAccess baseItemAcess, IInfoPartAccess otherItemAccess)
    {
      this._baseItemAcess = baseItemAcess;
      this._otherItemAccess = otherItemAccess;
    }

    /// <summary>
    /// IPropertyMatcher.
    /// </summary>
    public async Task<MatchOutputProperty> MatchAsync(int baseItemId, int otherItemId)
    {
      var baseFileProps = await this._baseItemAcess.GetFilePropertiesAsync(baseItemId);
      var otherFileProps = await this._otherItemAccess.GetFilePropertiesAsync(otherItemId);

      var baseFileName = Path.GetFileName(baseFileProps.Path);
      var otherFileName = Path.GetFileName(otherFileProps.Path);

      var qualification = baseFileName.Equals(otherFileName, StringComparison.OrdinalIgnoreCase)
        ? ComparisonQualification.Equal
        : ComparisonQualification.Neutral;

      var result = new MatchOutputProperty
      {
        Name = "file name",
        Value = $"{otherFileName}",
        Qualification = qualification
      };
      return result;
    }
  }
}

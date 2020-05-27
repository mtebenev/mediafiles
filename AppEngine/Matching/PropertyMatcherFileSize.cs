using ByteSizeLib;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Matches size of two files.
  /// </summary>
  public class PropertyMatcherFileSize : IPropertyMatcher
  {
    private readonly IInfoPartAccess _baseItemAcess;
    private readonly IInfoPartAccess _otherItemAccess;

    /// <summary>
    /// Ctor.
    /// </summary>
    public PropertyMatcherFileSize(IInfoPartAccess baseItemAcess, IInfoPartAccess otherItemAccess)
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

      var qualification = otherFileProps.Size < baseFileProps.Size
        ? ComparisonQualification.Better
        : otherFileProps.Size == baseFileProps.Size ? ComparisonQualification.Neutral : ComparisonQualification.Worse;

      var relativeSize = ByteSize.FromBytes(otherFileProps.Size - baseFileProps.Size);
      var result = new MatchOutputProperty
      {
        Name = "file size",
        Value = $"{ByteSize.FromBytes(otherFileProps.Size):#.##}",
        RelativeValue = relativeSize.ToString(relativeSize.Bytes > 0 ? "+#.##" : "#.##"),
        Qualification = qualification
      };
      return result;
    }
  }
}

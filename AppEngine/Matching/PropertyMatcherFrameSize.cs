using Mt.MediaFiles.AppEngine.Scanning;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Matches resolutions of two videos.
  /// </summary>
  public class PropertyMatcherFrameSize : IPropertyMatcher
  {
    private readonly IInfoPartAccess _baseItemAcess;
    private readonly IInfoPartAccess _otherItemAccess;

    /// <summary>
    /// Ctor.
    /// </summary>
    public PropertyMatcherFrameSize(IInfoPartAccess baseItemAcess, IInfoPartAccess otherItemAccess)
    {
      this._baseItemAcess = baseItemAcess;
      this._otherItemAccess = otherItemAccess;
    }

    /// <summary>
    /// IPropertyMatcher.
    /// </summary>
    public async Task<MatchOutputProperty> MatchAsync(int baseItemId, int otherItemId)
    {
      var baseInfoPart = await this._baseItemAcess.GetInfoPartAsync<InfoPartVideo>(baseItemId);
      var otherInfoPart = await this._otherItemAccess.GetInfoPartAsync<InfoPartVideo>(otherItemId);

      var qualification = otherInfoPart.VideoHeight > baseInfoPart.VideoHeight
        ? ComparisonQualification.Better
        : otherInfoPart.VideoHeight == baseInfoPart.VideoHeight ? ComparisonQualification.Neutral : ComparisonQualification.Worse;

      var result = new MatchOutputProperty
      {
        Name = "resolution",
        Value = $"{otherInfoPart.VideoWidth}x{otherInfoPart.VideoHeight}",
        Qualification = qualification
      };
      return result;
    }
  }
}

using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Scanning;
using System;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Matching
{
  /// <summary>
  /// Matches lengths of two video items.
  /// </summary>
  public class PropertyMatcherVideoLength : IPropertyMatcher
  {
    private readonly IInfoPartAccess _baseItemAcess;
    private readonly IInfoPartAccess _otherItemAccess;

    /// <summary>
    /// Ctor.
    /// </summary>
    public PropertyMatcherVideoLength(IInfoPartAccess baseItemAcess, IInfoPartAccess otherItemAccess)
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

      var marginSeconds = 1; // Ignore 1second (or less) difference.
      var lengthDiff = TimeSpan.FromMilliseconds(otherInfoPart.Duration).TotalSeconds - TimeSpan.FromMilliseconds(baseInfoPart.Duration).TotalSeconds;
      var qualification = ComparisonQualification.Equal;
      if(Math.Abs(lengthDiff) > marginSeconds)
      {
        qualification = lengthDiff > 0
          ? ComparisonQualification.Better
          : ComparisonQualification.Worse;
      }
      var diffFormat = lengthDiff > 0 ? "\\+hh\\:mm\\:ss" : "\\-hh\\:mm\\:ss";

      var result = new MatchOutputProperty
      {
        Name = "length",
        Value = TimeSpan.FromMilliseconds(otherInfoPart.Duration).ToString("hh\\:mm\\:ss"),
        RelativeValue = qualification == ComparisonQualification.Equal ? null : TimeSpan.FromSeconds(lengthDiff).ToString(diffFormat),
        Qualification = qualification
      };
      return result;
    }
  }
}

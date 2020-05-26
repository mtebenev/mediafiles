using Mt.MediaFiles.AppEngine.Scanning;
using System;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Matches lengths of two video items.
  /// </summary>
  public class OutputPropertyMatcherVideoLength : IOutputPropertyMatcher
  {
    private readonly IInfoPartAccess _baseItemAcess;
    private readonly IInfoPartAccess _otherItemAccess;

    /// <summary>
    /// Ctor.
    /// </summary>
    public OutputPropertyMatcherVideoLength(IInfoPartAccess baseItemAcess, IInfoPartAccess otherItemAccess)
    {
      this._baseItemAcess = baseItemAcess;
      this._otherItemAccess = otherItemAccess;
    }

    /// <summary>
    /// IOutputPropertyMatcher.
    /// </summary>
    public async Task<MatchOutputProperty> MatchAsync(int baseItemId, int otherItemId)
    {
      var baseInfoPart = await this._baseItemAcess.GetInfoPartAsync<InfoPartVideo>(baseItemId);
      var otherInfoPart = await this._otherItemAccess.GetInfoPartAsync<InfoPartVideo>(otherItemId);

      var lengthDiff = otherInfoPart.Duration - baseInfoPart.Duration;
      var qualification = lengthDiff > 0
        ? ComparisonQualification.Better
        : lengthDiff == 0 ? ComparisonQualification.Neutral : ComparisonQualification.Worse;

      var result = new MatchOutputProperty
      {
        Name = "length",
        Value = TimeSpan.FromMilliseconds(otherInfoPart.Duration).ToString(),
        Qualification = qualification
      };
      return result;
    }
  }
}

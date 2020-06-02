using Mt.MediaFiles.AppEngine.Matching;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Matching
{
  /// <summary>
  /// Match result processor for video files.
  /// </summary>
  public class MatchResultProcessorVideo
  {
    private readonly IInfoPartAccess _baseItemAccess;
    private readonly IInfoPartAccess _otherItemAccess;

    public MatchResultProcessorVideo(
      IInfoPartAccess baseItemAccess,
      IInfoPartAccess otherItemAccess
    )
    {
      this._baseItemAccess = baseItemAccess;
      this._otherItemAccess = otherItemAccess;
    }

    public async IAsyncEnumerable<MatchOutputGroup> ProcessAsync(MatchResult matchResult)
    {
      var propertyMatchers = new List<IPropertyMatcher>
      {
        new PropertyMatcherFileName(this._baseItemAccess, this._otherItemAccess),
        new PropertyMatcherFileSize(this._baseItemAccess, this._otherItemAccess),
        new PropertyMatcherVideoLength(this._baseItemAccess, this._otherItemAccess),
        new PropertyMatcherFrameSize(this._baseItemAccess, this._otherItemAccess),
      };

      foreach(var mg in matchResult.MatchGroups)
      {
        var outputGroup = await this.CreateOutputGroupAsync(propertyMatchers, mg);
        yield return outputGroup;
      }
    }

    private async Task<MatchOutputGroup> CreateOutputGroupAsync(List<IPropertyMatcher> propertyMatchers, MatchResultGroup resultGroup)
    {
      var baseItemFileProps = await this._baseItemAccess.GetFilePropertiesAsync(resultGroup.BaseItemId);
      var result = new MatchOutputGroup(baseItemFileProps.Path);
      foreach(var itemId in resultGroup.ItemIds)
      {
        var otherItemFileProps = await this._otherItemAccess.GetFilePropertiesAsync(itemId);
        var outputItem = new MatchOutputItem(otherItemFileProps.Path);
        foreach(var pm in propertyMatchers)
        {
          var propMatch = await pm.MatchAsync(resultGroup.BaseItemId, itemId);
          outputItem.AddProperty(propMatch);
        }
        result.AddItem(outputItem);
      }

      return result;
    }
  }
}

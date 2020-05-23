using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using StackExchange.Profiling;
using StackExchange.Profiling.Internal;

namespace Mt.MediaFiles.ClientApp.Cli.Ui
{
  /// <summary>
  /// Custom rendering for MiniProfiler.
  /// </summary>
  internal static class MiniProfilerExtensions
  {
    /// <summary>
    /// Custom rendering for MiniProfiler session.
    /// </summary>
    /// <returns></returns>
    public static string RenderPlainTextMf(this MiniProfiler profiler, bool htmlEncode = false)
    {
      if(profiler == null)
        return string.Empty;

      var text = StringBuilderCache.Get()
          .Append(htmlEncode ? WebUtility.HtmlEncode(Environment.MachineName) : Environment.MachineName)
          .Append(" at ")
          .Append(DateTime.UtcNow)
          .AppendLine();

      var timings = new Stack<Timing>();
      timings.Push(profiler.Root);

      while(timings.Count > 0)
      {
        var timing = timings.Pop();

        for(var i = 0; i < timing.Depth; i++)
        {
          text.Append(">>");
        }
        if(timing.Depth > 0)
        {
          text.Append(' ');
        }
        text.Append(htmlEncode ? WebUtility.HtmlEncode(timing.Name) : timing.Name)
            .Append(' ')
            .Append((timing.DurationMilliseconds ?? 0).ToString("###,##0.##"))
            .Append("ms");

        if(timing.HasCustomTimings)
        {
          text.AppendLine();
          foreach(var pair in timing.CustomTimings)
          {
            var type = pair.Key;
            var customTimings = pair.Value;

            text.Append("  * ")
                .Append(type)
                .Append(": ")
                .Append((customTimings.Sum(ct => ct.DurationMilliseconds) ?? 0).ToString("###,##0.##"))
                .Append("ms in ")
                .Append(customTimings.Count)
                .Append(" cmd")
                .Append(customTimings.Count == 1 ? string.Empty : "s")
                .AppendLine();
          }
        }

        text.AppendLine();

        if(timing.HasChildren)
        {
          var children = timing.Children;
          for(var i = children.Count - 1; i >= 0; i--)
            timings.Push(children[i]);
        }
      }

      return text.ToStringRecycle();
    }
  }
}

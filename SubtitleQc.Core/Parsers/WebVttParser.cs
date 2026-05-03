using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public sealed class WebVttParser : IParser
{
    private static readonly Regex _timeRegex = new Regex(
        @"^(?<start>\d{2}:\d{2}:\d{2}\.\d{3})\s*-->\s*(?<end>\d{2}:\d{2}:\d{2}\.\d{3})$",
        RegexOptions.Compiled);

    public IEnumerable<Cue> Parse(string content)
    {
        var blocks = content.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var block in blocks)
        {
            if (block.Trim() == "WEBVTT") continue;
            
            var lines = block.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int timeLineIndex = -1;
            
            for (int i = 0; i < lines.Length; i++)
            {
                if (_timeRegex.IsMatch(lines[i]))
                {
                    timeLineIndex = i;
                    break;
                }
            }

            if (timeLineIndex != -1)
            {
                var match = _timeRegex.Match(lines[timeLineIndex]);
                TimeSpan start = TimeSpan.ParseExact(match.Groups["start"].Value, @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
                TimeSpan end = TimeSpan.ParseExact(match.Groups["end"].Value, @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
                
                int textLineStart = timeLineIndex + 1;
                var textLines = new string[lines.Length - textLineStart];
                Array.Copy(lines, textLineStart, textLines, 0, textLines.Length);
                
                yield return new Cue(Guid.NewGuid().ToString("N"), start, end, textLines);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public sealed class SrtParser : IParser
{
    private static readonly Regex _timeRegex = new Regex(
        @"^(?<start>\d{2}:\d{2}:\d{2},\d{3})\s*-->\s*(?<end>\d{2}:\d{2}:\d{2},\d{3})$",
        RegexOptions.Compiled);

    public IEnumerable<Cue> Parse(string content)
    {
        var blocks = content.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var block in blocks)
        {
            var lines = block.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            if (lines.Length >= 3)
            {
                var match = _timeRegex.Match(lines[1]);
                if (match.Success)
                {
                    TimeSpan start = TimeSpan.ParseExact(match.Groups["start"].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);
                    TimeSpan end = TimeSpan.ParseExact(match.Groups["end"].Value, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);
                    
                    var textLines = new string[lines.Length - 2];
                    Array.Copy(lines, 2, textLines, 0, lines.Length - 2);
                    
                    yield return new Cue(Guid.NewGuid().ToString("N"), start, end, textLines);
                }
            }
        }
    }
}
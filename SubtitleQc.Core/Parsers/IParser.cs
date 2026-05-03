using System.Collections.Generic;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public interface IParser
{
    IEnumerable<Cue> Parse(string content);
}
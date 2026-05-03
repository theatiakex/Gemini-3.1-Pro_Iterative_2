using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class EmptyCueCheckRule : IQcRule
{
    public string Name => "EmptyCueCheck";

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        foreach (var cue in cues)
        {
            bool isEmpty = string.IsNullOrWhiteSpace(string.Join("", cue.Lines));
            var status = isEmpty ? QcStatus.Failed : QcStatus.Passed;
            yield return new QcResult(cue.Id, Name, status);
        }
    }
}
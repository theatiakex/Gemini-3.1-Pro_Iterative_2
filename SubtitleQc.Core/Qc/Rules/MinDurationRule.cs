using System;
using System.Collections.Generic;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MinDurationRule : IQcRule
{
    private readonly TimeSpan _threshold;

    public string Name => "MinDuration";

    public MinDurationRule(TimeSpan threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        foreach (var cue in cues)
        {
            var status = cue.Duration < _threshold ? QcStatus.Failed : QcStatus.Passed;
            yield return new QcResult(cue.Id, Name, status);
        }
    }
}
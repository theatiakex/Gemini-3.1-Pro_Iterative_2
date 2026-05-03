using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MaxCpsRule : IQcRule
{
    private readonly int _threshold;

    public string Name => "MaxCps";

    public MaxCpsRule(int threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        foreach (var cue in cues)
        {
            int totalChars = cue.Lines.Sum(line => line.Length);
            double cps = totalChars / cue.Duration.TotalSeconds;
            var status = cps > _threshold ? QcStatus.Failed : QcStatus.Passed;
            yield return new QcResult(cue.Id, Name, status);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MaxCplRule : IQcRule
{
    private readonly int _threshold;

    public string Name => "MaxCpl";

    public MaxCplRule(int threshold)
    {
        _threshold = threshold;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        foreach (var cue in cues)
        {
            bool exceeds = cue.Lines.Any(line => line.Length > _threshold);
            var status = exceeds ? QcStatus.Failed : QcStatus.Passed;
            yield return new QcResult(cue.Id, Name, status);
        }
    }
}
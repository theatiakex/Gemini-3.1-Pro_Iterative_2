using System.Collections.Generic;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class OverlapCheckRule : IQcRule
{
    public string Name => "OverlapCheck";

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        for (int i = 0; i < cues.Count; i++)
        {
            var cue = cues[i];
            var status = QcStatus.Passed;

            if (i > 0 && cue.Start < cues[i - 1].End)
            {
                status = QcStatus.Failed;
            }

            yield return new QcResult(cue.Id, Name, status);
        }
    }
}
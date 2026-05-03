using System;
using System.Collections.Generic;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class CrossShotBoundaryCheckRule : IQcRule
{
    private readonly IShotChangeProvider _shotChangeProvider;

    public string Name => "CrossShotBoundaryCheck";

    public CrossShotBoundaryCheckRule(IShotChangeProvider shotChangeProvider)
    {
        _shotChangeProvider = shotChangeProvider;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        var cuts = _shotChangeProvider.GetShotChangeTimestamps();

        foreach (var cue in cues)
        {
            var status = QcStatus.Passed;

            foreach (var cut in cuts)
            {
                if (cut > cue.Start && cut < cue.End)
                {
                    status = QcStatus.Failed;
                    break;
                }
            }

            yield return new QcResult(cue.Id, Name, status);
        }
    }
}
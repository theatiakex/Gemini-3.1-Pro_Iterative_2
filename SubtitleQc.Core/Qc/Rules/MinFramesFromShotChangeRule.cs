using System;
using System.Collections.Generic;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc.Rules;

public sealed class MinFramesFromShotChangeRule : IQcRule
{
    private readonly IShotChangeProvider _shotChangeProvider;
    private readonly int _thresholdFrames;

    public string Name => "MinFramesFromShotChange";

    public MinFramesFromShotChangeRule(IShotChangeProvider shotChangeProvider, int thresholdFrames)
    {
        _shotChangeProvider = shotChangeProvider;
        _thresholdFrames = thresholdFrames;
    }

    public IEnumerable<QcResult> Evaluate(IReadOnlyList<Cue> cues)
    {
        var cutFrames = _shotChangeProvider.GetShotChangeFrames();

        foreach (var cue in cues)
        {
            var status = QcStatus.Passed;

            if (cue.StartFrame.HasValue)
            {
                foreach (var cutFrame in cutFrames)
                {
                    int diff = Math.Abs(cue.StartFrame.Value - cutFrame);
                    if (diff < _thresholdFrames)
                    {
                        status = QcStatus.Failed;
                        break;
                    }
                }
            }

            yield return new QcResult(cue.Id, Name, status);
        }
    }
}
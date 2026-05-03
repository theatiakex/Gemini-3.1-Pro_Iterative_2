using System.Collections.Generic;

namespace SubtitleQc.Core.Models;

public sealed class QcReport
{
    public IReadOnlyList<QcResult> Results { get; }

    public QcReport(IReadOnlyList<QcResult> results)
    {
        Results = results;
    }
}
using System.Collections.Generic;

namespace SubtitleQc.Core.Models;

public sealed class QcResult
{
    public string CueId { get; }
    public string RuleName { get; }
    public QcStatus Status { get; }
    public string Message { get; }

    public QcResult(string cueId, string ruleName, QcStatus status, string message = "")
    {
        CueId = cueId;
        RuleName = ruleName;
        Status = status;
        Message = message;
    }
}
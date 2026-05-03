using System.Collections.Generic;
using System.Linq;
using SubtitleQc.Core.Models;
using SubtitleQc.Core.Qc.Abstractions;

namespace SubtitleQc.Core.Qc;

public sealed class RuleEngine
{
    private readonly IReadOnlyList<IQcRule> _rules;

    public RuleEngine(IEnumerable<IQcRule> rules)
    {
        _rules = rules.ToList();
    }

    public QcReport Evaluate(IReadOnlyList<Cue> cues)
    {
        var results = new List<QcResult>();
        
        foreach (var rule in _rules)
        {
            results.AddRange(rule.Evaluate(cues));
        }

        return new QcReport(results);
    }
}
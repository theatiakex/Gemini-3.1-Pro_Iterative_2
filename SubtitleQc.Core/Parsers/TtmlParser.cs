using System;
using System.Collections.Generic;
using System.Xml.Linq;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public sealed class TtmlParser : IParser
{
    private static readonly XNamespace Tt = "http://www.w3.org/ns/ttml";

    public IEnumerable<Cue> Parse(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) yield break;

        XDocument doc;
        try
        {
            doc = XDocument.Parse(content);
        }
        catch (Exception)
        {
            yield break;
        }

        var body = doc.Root?.Element(Tt + "body");
        if (body == null) yield break;

        foreach (var div in body.Elements(Tt + "div"))
        {
            foreach (var p in div.Elements(Tt + "p"))
            {
                var beginAttr = p.Attribute("begin");
                var endAttr = p.Attribute("end");

                if (beginAttr != null && endAttr != null &&
                    TryParseTime(beginAttr.Value, out TimeSpan start) &&
                    TryParseTime(endAttr.Value, out TimeSpan end))
                {
                    string id = p.Attribute("id")?.Value ?? Guid.NewGuid().ToString("N");
                    string[] lines = ExtractLines(p);
                    yield return new Cue(id, start, end, lines);
                }
            }
        }
    }

    private static bool TryParseTime(string value, out TimeSpan time)
    {
        return TimeSpan.TryParse(value, out time);
    }

    private static string[] ExtractLines(XElement pElement)
    {
        var lines = new List<string>();
        var currentLine = new System.Text.StringBuilder();

        foreach (var node in pElement.Nodes())
        {
            if (node is XText textNode)
            {
                currentLine.Append(textNode.Value);
            }
            else if (node is XElement element && element.Name == Tt + "br")
            {
                lines.Add(currentLine.ToString().Trim());
                currentLine.Clear();
            }
            else if (node is XElement spanElement && spanElement.Name == Tt + "span")
            {
                 currentLine.Append(spanElement.Value);
            }
        }

        if (currentLine.Length > 0 || lines.Count == 0)
        {
            lines.Add(currentLine.ToString().Trim());
        }

        return lines.ToArray();
    }
}
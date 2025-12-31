using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.OpenIddict;

public class HashSetStringConverter
{
    public readonly static HashSetStringConverter Instance = new HashSetStringConverter();

    private readonly Func<string, bool> _filter;
    public HashSetStringConverter(Func<string, bool> filter = null)
    {
        _filter = filter;
    }

    public string Convert(HashSet<string> sourceMember)
    {
        if (sourceMember == null || !sourceMember.Any())
        {
            return null;
        }
        return sourceMember.Where(x => x != "\r" && x != "\n" && x != "\r\n").Distinct().Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
    }

    public HashSet<string> Convert(string sourceMember)
    {
        var list = new List<string>();
        if (!string.IsNullOrWhiteSpace(sourceMember))
        {
            sourceMember = sourceMember.Trim();
            foreach (var item in sourceMember.Trim().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Distinct())
            {
                list.Add(item);
            }
        }

        if (_filter != null)
        {
            list = list.Where(_filter).ToList();
        }

        return new HashSet<string>(list
            .Where(x => x != "\r" && x != "\n" && x != "\r\n")
            .Select(x => x.Replace("\r", "").Replace("\n", "").Replace("\r\n", ""))
            .Distinct());
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Piovra.Sql.Core;

public partial class DbConnStringObject {
    DbConnStringObject(string connString) => ConnString = ARG.NotNullOrEmpty(connString, nameof(connString));

    public string ConnString { get; }
    public Dictionary<string, string> Map { get; } = new Dictionary<string, string>();

    public bool ContainsKey(string key) => Map.ContainsKey(Key(key));
    public string Value(string key) => Map[Key(key)];
    public string FormatPair(string key, char separator = '=') => $"{key}{separator}{Value(key)}";
    static string Key(string key) => key.ToLower();

    public static DbConnStringObject Parse(string connString) {
        var result = new DbConnStringObject(connString);
        var ms = _regex.Matches(connString);
        foreach (Match m in ms.Cast<Match>()) {
            result.Map.Add(m.Groups[1].Value.ToLower(), m.Groups[2].Value);
        }
        return result;
    }

    static readonly Regex _regex = MyRegex();

    [GeneratedRegex("(?<key>[^=;]+)=(?<value>[^;]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex MyRegex();
}

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Piovra.Sql.Core {
    public class ConnStringObject {
        ConnStringObject(string connectionString) => ConnectionString = connectionString;
        public string ConnectionString { get; }
        public Dictionary<string, string> Map { get; } = new Dictionary<string, string>();
        public bool ContainsKey(string key) => Map.ContainsKey(Key(key));
        public string Value(string key) => Map[Key(key)];
        public string FormatPair(string key, char separator = '=') => $"{key}{separator}{Value(key)}";
        string Key(string key) => key.ToLower();

        public static ConnStringObject Parse(string connectionString) {
            var res = new ConnStringObject(connectionString);
            var ms = Regex.Matches(connectionString, @"(?<key>[^=;]+)=(?<value>[^;]+)", RegexOptions.IgnoreCase);
            foreach (Match m in ms) {
                res.Map.Add(m.Groups[1].Value.ToLower(), m.Groups[2].Value);
            }
            return res;
        }
    }
}

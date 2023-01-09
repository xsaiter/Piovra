using System.Text;

namespace Piovra.DapperExtensions;

public class Naming {
    public static string ConvertFromCamelToSnakeCase(string s, bool useAbbreviations = true) {
        var res = new StringBuilder();
        var n = s.Length;
        for (var i = 0; i < n; ++i) {
            if (i - 1 >= 0) {
                if (IsLower(s[i - 1]) && IsUpper(s[i])) {
                    AppendUnderscore();
                } else if (useAbbreviations) {
                    if (i + 1 < n) {
                        if (IsUpper(s[i - 1]) && IsUpper(s[i]) && IsLower(s[i + 1])) {
                            AppendUnderscore();
                        }
                    }
                }
            }
            res.Append(ToLower(s[i]));
        }
        return res.ToString();

        void AppendUnderscore() => res.Append('_');
        static bool IsUpper(char c) => char.IsUpper(c);
        static bool IsLower(char c) => char.IsLower(c);
        static char ToLower(char c) => char.ToLower(c);
    }
}
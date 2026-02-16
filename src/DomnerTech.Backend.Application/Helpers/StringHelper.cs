using Microsoft.Extensions.Primitives;
using System.Text.RegularExpressions;

namespace DomnerTech.Backend.Application.Helpers;

public static partial class StringHelper
{
    extension(string o)
    {
        public string ToSnakeCase() =>
            SnakeCase().Replace(o, "$1_$2").ToLower();

        public string ToCamelCase() =>
            o.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s[1..])
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);

        public StringValues ToStringValues()
        {
            return new StringValues(o);
        }
    }

    [GeneratedRegex(@"(\w)([A-Z])")]
    private static partial Regex SnakeCase();

    private static readonly string[] Separator = ["_"];
}
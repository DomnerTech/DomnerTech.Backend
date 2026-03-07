using System.Text.RegularExpressions;

namespace DomnerTech.Backend.Api.Transformer;

public sealed partial class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) return null;

        return KebabCase().Replace(value.ToString() ?? string.Empty, "$1-$2")
            .ToLowerInvariant();
    }

    [GeneratedRegex("([a-z])([A-Z])", RegexOptions.CultureInvariant)]
    private static partial Regex KebabCase();
}
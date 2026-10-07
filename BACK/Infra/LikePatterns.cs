namespace Pokedex.Api.Infra;

internal static class LikePatterns
{
    public static string Contains(string value)
    {
        var escaped = value
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("%", "\\%", StringComparison.Ordinal)
            .Replace("_", "\\_", StringComparison.Ordinal);

        return $"%{escaped}%";
    }
}

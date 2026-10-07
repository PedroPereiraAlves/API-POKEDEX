using Pokedex.Api.Infra;

namespace Pokedex.Api.Tests;

public class LikePatternsTests
{
    [Fact]
    public void Contains_EscapesLikeWildcards()
    {
        Assert.Equal("%pi\\%ka\\_chu\\\\%", LikePatterns.Contains("pi%ka_chu\\"));
    }
}

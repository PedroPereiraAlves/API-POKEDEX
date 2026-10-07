using System.Text.Json.Serialization;

namespace Pokedex.ImageSync.Models;

public class Sprite
{
    [JsonPropertyName("front_default")]
    public string? FrontDefault { get; set; }
}

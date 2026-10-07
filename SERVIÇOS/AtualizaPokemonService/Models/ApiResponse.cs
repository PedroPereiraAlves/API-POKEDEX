using System.Text.Json.Serialization;

namespace Pokedex.ImageSync.Models;

public class ApiResponse
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("sprites")]
    public Sprite? Sprites { get; set; }
}

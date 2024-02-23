using System.Text.Json.Serialization;

public class Sprite
{
    [JsonPropertyName("front_default")]
    public string FrontDefault { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pokedex.Api.Contracts;

public class PokemonAddViewModel : IValidatableObject
{
    [JsonPropertyName("nomePokemon")]
    [MaxLength(100, ErrorMessage = "O nome do pokemon deve ter no máximo 100 caracteres.")]
    public string NomePokemon { get; set; } = "";

    [JsonPropertyName("sexoPokemon")]
    [Range(0, 1, ErrorMessage = "O sexo do pokemon deve ser 0 (homem) ou 1 (mulher).")]
    public int SexoPokemon { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(NomePokemon))
        {
            yield return new ValidationResult(
                "O nome do pokemon é obrigatório.",
                [nameof(NomePokemon)]);
        }
    }
}

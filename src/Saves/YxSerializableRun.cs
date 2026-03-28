using System.Text.Json.Serialization;
using Yixian.Characters;

namespace Yixian.Saves;

/// <summary>Run save of Yixian Mod.</summary>
public partial class YxSerializableRun
{
    /// <summary>Last selected character in Heptastar Pavilion.</summary>
    [JsonPropertyName("character_heptastar_pavilion")]
    public YxHeptastarPavilionCharacter? CharacterHeptastarPavilion;

    /// <summary>RNG constructor paramter.</summary>
    [JsonPropertyName("hexagram_rng_counter")]
    public int? HexagramRngCounter;
}

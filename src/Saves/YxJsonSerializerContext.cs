using System.Text.Json;
using System.Text.Json.Serialization;
using MegaCrit.Sts2.Core.Saves;
using Yixian.Characters;

namespace Yixian.Saves;

/// <summary>Json serilaizer context.</summary>
[JsonSerializable(typeof(YxSerializableRun))]
[JsonSourceGenerationOptions(
    WriteIndented = true,
    IncludeFields = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
    UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
    GenerationMode = JsonSourceGenerationMode.Metadata,
    Converters = [
        typeof(SnakeCaseJsonStringEnumConverter<YxHeptastarPavilionCharacter>),
    ]
)]
public partial class YxJsonSerializerContext : JsonSerializerContext { }

using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;

/// <summary>
/// Patches <see cref="ModelDb.AllCharacters"/>.
/// </summary>
[HarmonyPatch(typeof(ModelDb), nameof(ModelDb.AllCharacters), MethodType.Getter)]
public static class AllCharactersPostfix
{
    /// <summary>
    /// Appends <see cref="ModelDb.AllCharacters"/>.
    /// </summary>
    [HarmonyPostfix]
    public static IEnumerable<CharacterModel> Postfix(IEnumerable<CharacterModel> __result) => __result.Concat([
        ModelDb.Character<YxHeptastarPavilion>()
    ]);
}

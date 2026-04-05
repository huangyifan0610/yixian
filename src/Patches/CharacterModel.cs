using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;

namespace Yixian.Patches;

/// <summary>Extension to <see cref="CharacterModel"/>.</summary>
public static class CharacterModelExtension
{
    /// <summary>
    /// If <paramref name="character"/> is <see cref="YxHeptastarPavilion"/>, 
    /// applies <paramref name="hp"/> on it and assign the result to <paramref name="__result"/>.
    /// </summary>
    /// <returns>Returns false if <paramref name="__result"/> is assigned.</returns>
    internal static bool Patch<T>(this CharacterModel character, ref T __result, Func<YxHeptastarPavilion, T> hp)
    {
        if (character is YxHeptastarPavilion characterHp)
        {
            __result = hp.Invoke(characterHp);
            return false;
        }
        else
        {
            return true;
        }
    }
}

/// <summary>See <see cref="CharacterModel.Title"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.Title), MethodType.Getter)]
public static class CharacterModel_Title
{

    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => new LocString("characters", hp.Character.Uppercase() + ".title")
    );
}

/// <summary>See <see cref="CharacterModel.TitleObject"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.TitleObject), MethodType.Getter)]
public static class CharacterModel_TitleObject
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => new LocString("characters", hp.Character.Uppercase() + ".titleObject")
    );
}

/// <summary>See <see cref="CharacterModel.PronounObject"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.PronounObject), MethodType.Getter)]
public static class CharacterModel_PronounObject
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => new LocString("characters", hp.Character.Uppercase() + ".pronounObject")
    );
}

/// <summary>See <see cref="CharacterModel.PossessiveAdjective"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.PossessiveAdjective), MethodType.Getter)]
public static class CharacterModel_PossessiveAdjective
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => new LocString("characters", hp.Character.Uppercase() + ".possessiveAdjective")
    );
}

/// <summary>See <see cref="CharacterModel.PronounPossessive"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.PronounPossessive), MethodType.Getter)]
public static class CharacterModel_PronounPossessive
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => new LocString("characters", hp.Character.Uppercase() + ".pronounPossessive")
    );
}

/// <summary>See <see cref="CharacterModel.PronounSubject"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.PronounSubject), MethodType.Getter)]
public static class CharacterModel_PronounSubject
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => new LocString("characters", hp.Character.Uppercase() + ".pronounSubject")
    );
}

/// <summary>See <see cref="CharacterModel.CardsModifierTitle"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CardsModifierTitle), MethodType.Getter)]
public static class CharacterModel_CardsModifierTitle
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => new LocString("characters", hp.Character.Uppercase() + ".cardsModifierTitle")
    );
}

/// <summary>See <see cref="CharacterModel.CardsModifierDescription"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CardsModifierDescription), MethodType.Getter)]
public static class CharacterModel_CardsModifierDescription
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => new LocString("characters", hp.Character.Uppercase() + ".cardsModifierDescription")
    );
}

/// <summary>See <see cref="CharacterModel.EventDeathPreventionLine"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.EventDeathPreventionLine), MethodType.Getter)]
public static class CharacterModel_EventDeathPreventionLine
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => new LocString("characters", hp.Character.Uppercase() + ".eventDeathPreventionLine")
    );
}

/// <summary>See <see cref="CharacterModel.CharacterSelectTitle"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CharacterSelectTitle), MethodType.Getter)]
public static class CharacterModel_CharacterSelectTitle
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => hp.Character.Uppercase() + ".title"
    );
}

/// <summary>See <see cref="CharacterModel.CharacterSelectDesc"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CharacterSelectDesc), MethodType.Getter)]
public static class CharacterModel_CharacterSelectDesc
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => hp.Character.Uppercase() + ".description"
    );
}

/// <summary>See <see cref="CharacterModel.CharacterSelectBg"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CharacterSelectBg), MethodType.Getter)]
public static class CharacterModel_CharacterSelectBg
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => SceneHelper.GetScenePath("screens/char_select/char_select_bg_" + hp.Character.Lowercase())
    );
}

/// <summary>See <see cref="CharacterModel.VisualsPath"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), "VisualsPath", MethodType.Getter)]
public static class CharacterModel_VisualsPath
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => SceneHelper.GetScenePath("creature_visuals/" + hp.Character.Lowercase())
    );
}

/// <summary>See <see cref="CharacterModel.IconPath"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), "IconPath", MethodType.Getter)]
public static class CharacterModel_IconPath
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => SceneHelper.GetScenePath("ui/character_icons/" + hp.Character.Lowercase() + "_icon")
    );
}

/// <summary>See <see cref="CharacterModel.EnergyCounterPath"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.EnergyCounterPath), MethodType.Getter)]
public static class CharacterModel_EnergyCounterPath
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => __instance.Patch(ref __result,
        // TODO: Patch CharacterModel.EnergyCounterPath
        // hp => SceneHelper.GetScenePath("combat/energy_counters/" + hp.Character.Lowercase() + "_energy_counter")
        hp => SceneHelper.GetScenePath("combat/energy_counters/ironclad_energy_counter")
    );
}

/// <summary>See <see cref="CharacterModel.MerchantAnimPath"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.MerchantAnimPath), MethodType.Getter)]
public static class CharacterModel_MerchantAnimPath
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => SceneHelper.GetScenePath("merchant/characters/" + hp.Character.Lowercase() + "_merchant")
    );
}

/// <summary>See <see cref="CharacterModel.RestSiteAnimPath"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.RestSiteAnimPath), MethodType.Getter)]
public static class CharacterModel_RestSiteAnimPath
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => __instance.Patch(ref __result,
        hp => SceneHelper.GetScenePath("rest_site/characters/" + hp.Character.Lowercase() + "_rest_site")
    );
}

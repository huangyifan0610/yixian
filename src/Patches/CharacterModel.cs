using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;

namespace Yixian.Patches;

internal static class Utility
{
    /// <summary>
    /// If <paramref name="__instance"/> is <see cref="YxHeptastarPavilion"/>, 
    /// applies <paramref name="hp"/> on it and assign the result to <paramref name="__result"/>.
    /// </summary>
    /// <returns>Returns false if <paramref name="__result"/> is assigned.</returns>
    internal static bool Patch<T>(ref T __result, CharacterModel __instance, Func<YxHeptastarPavilion, T> hp)
    {
        if (__instance is YxHeptastarPavilion yxHeptastarPavilion)
        {
            __result = hp.Invoke(yxHeptastarPavilion);
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
public static class Title
{

    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => new LocString("characters", hp.CharacterUppercase + ".title")
    );
}

/// <summary>See <see cref="CharacterModel.TitleObject"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.TitleObject), MethodType.Getter)]
public static class TitleObject
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => new LocString("characters", hp.CharacterUppercase + ".titleObject")
    );
}

/// <summary>See <see cref="CharacterModel.PronounObject"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.PronounObject), MethodType.Getter)]
public static class PronounObject
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => new LocString("characters", hp.CharacterUppercase + ".pronounObject")
    );
}

/// <summary>See <see cref="CharacterModel.PossessiveAdjective"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.PossessiveAdjective), MethodType.Getter)]
public static class PossessiveAdjective
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => new LocString("characters", hp.CharacterUppercase + ".possessiveAdjective")
    );
}

/// <summary>See <see cref="CharacterModel.PronounPossessive"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.PronounPossessive), MethodType.Getter)]
public static class PronounPossessive
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => new LocString("characters", hp.CharacterUppercase + ".pronounPossessive")
    );
}

/// <summary>See <see cref="CharacterModel.PronounSubject"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.PronounSubject), MethodType.Getter)]
public static class PronounSubject
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => new LocString("characters", hp.CharacterUppercase + ".pronounSubject")
    );
}

/// <summary>See <see cref="CharacterModel.CardsModifierTitle"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CardsModifierTitle), MethodType.Getter)]
public static class CardsModifierTitle
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => new LocString("characters", hp.CharacterUppercase + ".cardsModifierTitle")
    );
}

/// <summary>See <see cref="CharacterModel.CardsModifierDescription"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CardsModifierDescription), MethodType.Getter)]
public static class CardsModifierDescription
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => new LocString("characters", hp.CharacterUppercase + ".cardsModifierDescription")
    );
}

/// <summary>See <see cref="CharacterModel.EventDeathPreventionLine"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.EventDeathPreventionLine), MethodType.Getter)]
public static class EventDeathPreventionLine
{
    [HarmonyPrefix]
    public static bool Prefix(ref LocString __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => new LocString("characters", hp.CharacterUppercase + ".eventDeathPreventionLine")
    );
}

/// <summary>See <see cref="CharacterModel.CharacterSelectTitle"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CharacterSelectTitle), MethodType.Getter)]
public static class CharacterSelectTitle
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => hp.CharacterUppercase + ".title"
    );
}

/// <summary>See <see cref="CharacterModel.CharacterSelectDesc"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CharacterSelectDesc), MethodType.Getter)]
public static class CharacterSelectDesc
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => hp.CharacterUppercase + ".description"
    );
}


/// <summary>See <see cref="CharacterModel.CharacterSelectBg"/>.</summary>
[HarmonyPatch(typeof(CharacterModel), nameof(CharacterModel.CharacterSelectBg), MethodType.Getter)]
public static class CharacterSelectBg
{
    [HarmonyPrefix]
    public static bool Prefix(ref string __result, CharacterModel __instance) => Utility.Patch(ref __result, __instance,
        hp => SceneHelper.GetScenePath("screens/char_select/char_select_bg_" + hp.CharacterLowercase)
    );
}

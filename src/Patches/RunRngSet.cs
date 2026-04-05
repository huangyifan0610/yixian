using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace Yixian.Patches;

/// <summary>Extension to <see cref="RunRngSet"/></summary>
public static class RunRngSetExtension
{
    /// <summary>RNG for hexagram effect.</summary>
    private readonly static ConditionalWeakTable<RunRngSet, Rng> _hexagramRng = [];

    internal static Rng CreateRngForHexagram(this RunRngSet rngs) => new(
        seed: rngs.Seed,
        counter: 0
    );

    internal static Rng CreateRngForHexagramWithCounter(this RunRngSet rngs) => new(
        seed: rngs.Seed,
        // Loads from save.
        counter: SaveManager.Instance.GetYxSerializableRun()?.HexagramRngCounter ?? 0
    );

    /// <summary>Returns RNG for hexagram effect.</summary>
    public static Rng GetRngForHexagram(this RunRngSet rngs) => _hexagramRng.GetValue(rngs, CreateRngForHexagram);

    /// <summary>Sets RNG for hexagram effect.</summary>
    public static void SetRngForHexagram(this RunRngSet rngs, Rng rng) => _hexagramRng.AddOrUpdate(rngs, rng);
}

/// <summary>Patches <see cref="RunRngSet.FromSave"/></summary>
[HarmonyPatch(typeof(RunRngSet), nameof(RunRngSet.FromSave))]
public static class RunRngSet_FromSave
{
    [HarmonyPostfix]
    public static void Postfix(RunRngSet __result) => __result.SetRngForHexagram(__result.CreateRngForHexagramWithCounter());
}

/// <summary>Patches <see cref="RunRngSet.LoadFromSerializable"/></summary>
[HarmonyPatch(typeof(RunRngSet), nameof(RunRngSet.LoadFromSerializable))]
public static class RunRngSet_LoadFromSerializable
{
    [HarmonyPostfix]
    public static void Postfix(RunRngSet __instance) => __instance.SetRngForHexagram(__instance.CreateRngForHexagramWithCounter());
}

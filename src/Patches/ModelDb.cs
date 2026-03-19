using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using Yixian.Cards.HeptastarPavilion;
using Yixian.Relics;

namespace Yixian.Patches;

/// <summary>
/// Patches <see cref="ModelDb.AllSharedCardPools"/>.
/// </summary>
[HarmonyPatch(typeof(ModelDb), nameof(ModelDb.AllSharedCardPools), MethodType.Getter)]
public static class AllSharedCardPoolsPostfix
{
    private static readonly CardPoolModel[] _sharedCardPools = [
        ModelDb.CardPool<HeptastarPavilionCardPool>()
    ];

    /// <summary>
    /// Appends <see cref="_sharedCardPools"/> to <see cref="ModelDb.AllSharedCardPools"/>.
    /// </summary>
    [HarmonyPostfix]
    public static IEnumerable<CardPoolModel> Postfix(IEnumerable<CardPoolModel> __result)
    {
        return __result.Concat(_sharedCardPools).Distinct();
    }
}

/// <summary>
/// Patches <see cref="ModelDb.AllRelicPools"/>.
/// </summary>
[HarmonyPatch(typeof(ModelDb), nameof(ModelDb.AllRelicPools), MethodType.Getter)]
public static class AllSharedRelicPoolsPostfix
{
    private static readonly RelicPoolModel[] _relicPools = [
        ModelDb.RelicPool<HeptastarPavilionRelicPool>()
    ];

    /// <summary>
    /// Appends <see cref="_relicPools"/> to <see cref="ModelDb.AllRelicPools"/>.
    /// </summary>
    [HarmonyPostfix]
    public static IEnumerable<RelicPoolModel> Postfix(IEnumerable<RelicPoolModel> __result)
    {
        return __result.Concat(_relicPools).Distinct();
    }
}
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using Yixian.Cards.HeptastarPavilion;

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
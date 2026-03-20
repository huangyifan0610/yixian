using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Yixian.Powers;

namespace Yixian.Patches;

/// <summary>
/// Extension of <see cref="CardModel"/> that provides additional data.
/// </summary>
public static class CardModelExtension
{
    /// <summary>
    /// Extension data for each <see cref="CardModel"/> instance.
    /// </summary>
    public class Externsion
    {
        /// <summary>
        /// Whether the card is on Star Point.
        /// </summary>
        public bool IsOnStarPoint = false;
    }

    /// <summary>
    /// Extension data for each <see cref="CardModel"/> instance.
    /// </summary>
    public static readonly ConditionalWeakTable<CardModel, Externsion> ExternsionTable = [];

    /// <summary>
    /// Returns true if the card is on Star Point.
    /// </summary>
    public static bool IsOnStarPoint(this CardModel cardModel)
    {
        return (cardModel.Pile?.Type) switch
        {
            PileType.Hand => cardModel.IsOnStarPointForHand(),
            PileType.Play => ExternsionTable.TryGetValue(cardModel, out Externsion? ext) && ext.IsOnStarPoint,
            _ => true,
        };
    }

    /// <summary>
    /// Returns true if the card is on Star Point (only for card in hands).
    /// </summary>
    private static bool IsOnStarPointForHand(this CardModel cardModel)
    {
        if (cardModel.Pile?.Type != PileType.Hand)
        {
            throw new ArgumentException($"expect hand pile, but get {cardModel.Pile?.Type}");
        }

        int index = cardModel.Pile.Cards.IndexOf(cardModel);
        var power = cardModel.Owner?.Creature?.GetPower<StarPointPower>();
        return power == null ? StarPointPower.IsDefaultStarPoint(index) : power.IsStarPoint(index);
    }
}

/// <summary>
/// Checkes whether the card is on Star Point before playing the card. 
/// </summary>
[HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
public static class CardModelOnPlayWrapperPrefix
{
    /// <summary>
    /// Patches <see cref="CardModel.OnPlayWrapper"/> that adds star point for the card before it's played.
    /// </summary>
    [HarmonyPrefix]
    public static async void Prefix(CardModel __instance, PlayerChoiceContext choiceContext, Creature? target, bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals = false)
    {
        Creature? owner = __instance.Owner?.Creature;
        CardPile? pile = __instance.Pile;

        // Star points only exist in hand pile.
        if (owner != null && pile?.Type == PileType.Hand)
        {
            // NOTE: We must get index before applying the power.
            // 
            // It's weired that, after applying StarPointPower for the first time, 
            // the card is removed from our hand, then IndexOf returns -1.
            int index = pile.Cards.IndexOf(__instance);

            // Applies Star Point power if it not exists.
            var power = owner.GetPower<StarPointPower>()
                ?? await PowerCmd.Apply<StarPointPower>(owner, 1, owner, __instance);

            if (power != null && power.IsStarPoint(index))
            {
                // Remebers that the card is on Star Point.
                CardModelExtension.ExternsionTable.GetValue(__instance, key => new()).IsOnStarPoint = true;
            }
            else if (CardModelExtension.ExternsionTable.TryGetValue(__instance, out var ext))
            {
                // Resets if the card is not on Star Point.
                ext.IsOnStarPoint = false;
            }
        }
    }
}

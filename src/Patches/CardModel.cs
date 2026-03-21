using System;
using System.Linq;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
            _ => false,
        };
    }

    /// <summary>
    /// Returns true if the card is on Star Point (only for card in hands).
    /// </summary>
    internal static bool IsOnStarPointForHand(this CardModel cardModel)
    {
        if (cardModel.Pile?.Type != PileType.Hand)
        {
            throw new ArgumentException($"expect hand pile, but get {cardModel.Pile?.Type}");
        }

        // Polaris Citta Dharma allows every slot to be star point. 
        if (cardModel.Owner?.Creature?.HasPower<PolarisCittaDharmaPower>() ?? false)
        {
            return true;
        }

        // Inspect the position of the card in hand.
        int position = cardModel.Pile.Cards.IndexOf(cardModel);
        if (StarPointPower.IsDefaultStarPoint(position))
        {
            return true;
        }
        else
        {
            var starPointPower = cardModel.Owner?.Creature?.GetPower<StarPointPower>();
            return starPointPower != null && starPointPower.IsStarPoint(position);
        }
    }

    /// <summary>
    /// Returns true if the card has been played at least once in the combat.
    /// </summary>
    internal static bool HasPlayed(this CardModel cardModel) => CombatManager.Instance.History.Entries.Any(entry => entry is CardPlayFinishedEntry cardEntry && cardEntry.CardPlay.Card == cardModel);
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
    public static void Prefix(CardModel __instance, PlayerChoiceContext choiceContext, Creature? target, bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals = false)
    {
        // Star points only exist in hand pile.
        if (__instance.Pile?.Type == PileType.Hand && __instance.IsOnStarPointForHand())
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

using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Yixian.Powers;

namespace Yixian.Patches;

/// <summary>Extension class for <see cref="CardModel"/>.</summary>
public static class CardModelExtension
{
    /// <summary>Extended data for <see cref="CardModel"/>.</summary>
    public class Data
    {
        /// <summary>Position in hand before the card is played.</summary>
        public int IndexInHand = -1;
    }

    /// <summary>
    /// Extension data for each <see cref="CardModel"/> instance.
    /// </summary>
    public static readonly ConditionalWeakTable<CardModel, Data> Table = [];

    /// <summary>Returns the card's position in hand.</summary>
    /// <remarks>Alwasy returns -1 if the card is not in hand or play pile.</remarks>
    public static int IndexInHand(this CardModel cardModel) => (cardModel.Pile?.Type) switch
    {
        PileType.Hand => cardModel.Pile.Cards.IndexOf(cardModel),
        PileType.Play => Table.TryGetValue(cardModel, out var externsion) ? externsion.IndexInHand : -1,
        _ => -1,
    };

    /// <summary>Retruns true if the <paramref name="cardModel"/> is on star point.</summary>
    public static bool TestStarPoint(this CardModel cardModel)
    {
        int index = cardModel.IndexInHand();
        if (0 <= index && index <= CardPile.maxCardsInHand)
        {
            var power = cardModel.Owner?.Creature?.GetPower<YxStarPointPower>() ?? YxStarPointPower.DEFAULT;
            return power[index];
        }

        return false;
    }
}

/// <summary>
/// Checkes whether the card is on Star Point before playing the card. 
/// </summary>
[HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
public static class OnPlayWrapper
{
    /// <summary>
    /// Patches <see cref="CardModel.OnPlayWrapper"/> that updates <see cref="CardModelExtension.Data"/>>.
    /// </summary>
    [HarmonyPrefix]
    public static void Prefix(CardModel __instance, PlayerChoiceContext choiceContext, Creature? target, bool isAutoPlay, ResourceInfo resources, bool skipCardPileVisuals = false)
    {
        if (__instance.Pile?.Type == PileType.Hand)
        {
            // Remembers the card's position in hand before it's played.
            var extension = CardModelExtension.Table.GetValue(__instance, _ => new());
            extension.IndexInHand = __instance.Pile.Cards.IndexOf(__instance);
        }
    }
}

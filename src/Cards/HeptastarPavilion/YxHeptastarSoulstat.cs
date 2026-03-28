using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Heptastar Soulstat.</summary>
public sealed class YxHeptastarSoulstat() : YxCardModel(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Exhaust.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];

    /// <summary>Character Exclusive.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.CharacterExclusiveJiangXiming];

    /// <summary>If the number of cards played == 7, end this turn and takes an extra turn.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(7),
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedVar("CalculatedCards").WithMultiplier(CalculatedCardsMultiplier),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.CharacterExclusiveJiangXiming.GetHoverTip(),
    ];

    /// <summary>Glow if we have star power.</summary>
    protected override bool ShouldGlowGoldInternal => IsPlayable;

    /// <summary>Can only be played if the number of cards played == 7.</summary>
    protected override bool IsPlayable =>
        ((CalculatedVar)DynamicVars["CalculatedCards"]).Calculate(null) == DynamicVars.Cards.BaseValue;

    /// <summary>Multiplier for <see cref="CalculatedVar"/></summary>
    private static decimal CalculatedCardsMultiplier(CardModel card, Creature? target)
    {
        ArgumentNullException.ThrowIfNull(card.CombatState, nameof(card.CombatState));
        return CombatManager.Instance.History.Entries.Count(entry => entry.RoundNumber == card.CombatState.RoundNumber && entry is CardPlayFinishedEntry);
    }

    /// <summary>Removes Exhaust keyword.</summary>
    protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Exhaust);

    /// <summary>If the number of cards played == 7, end this turn and takes an extra turn.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxHeptastarSoulstatPower>(Owner.Creature, 1, Owner.Creature, this);
        PlayerCmd.EndTurn(Owner, canBackOut: false);
    }
}

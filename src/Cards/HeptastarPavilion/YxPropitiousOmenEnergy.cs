using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Propitious Omen - Energy.</summary>
public sealed class YxPropitiousOmenEnergy() : YxCardModel(0, CardType.Skill, CardRarity.Event, TargetType.Self, false)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Exhaust.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    /// <summary>Gain energy.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(1),
    ];

    /// <summary>Gain energy.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}

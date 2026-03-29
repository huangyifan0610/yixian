using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Propitious Omen - Hexagram.</summary>
public sealed class YxPropitiousOmenHexagram() : YxCardModel(0, CardType.Skill, CardRarity.Event, TargetType.Self, false)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Exhaust.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    /// <summary>Gain hexagram.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxHexagramPower>(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Gain hexagram.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxHexagramPower>(
            Owner.Creature,
            DynamicVars[nameof(YxHexagramPower)].BaseValue,
            Owner.Creature,
            this
        );
    }
}

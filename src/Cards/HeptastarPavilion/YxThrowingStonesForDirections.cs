using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Throwing Stones For Directions.</summary>
public sealed class YxThrowingStonesForDirections() : YxCardModel(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Apply random weak; Apply random vulnerable.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<WeakPower>("MinWeakPower", 0),
        new PowerVar<WeakPower>("MaxWeakPower", 2),
        new PowerVar<VulnerablePower>("MinVulnerablePower", 0),
        new PowerVar<VulnerablePower>("MaxVulnerablePower", 2),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have enough hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPower<YxHexagramPower>()?.Amount >= 2;

    /// <summary>Apply more weak; Apply more vulnerable.</summary>
    protected override void OnUpgrade()
    {
        DynamicVars["MaxWeakPower"].UpgradeValueBy(1);
        DynamicVars["MaxVulnerablePower"].UpgradeValueBy(1);
    }

    /// <summary>Apply random weak; Apply random vulnerable.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<WeakPower>(
            cardPlay.Target,
            Owner.Creature.GetPower<YxHexagramPower>().Range(
                RunState,
                DynamicVars["MinWeakPower"].IntValue,
                DynamicVars["MaxWeakPower"].IntValue,
                out bool _
            ),
            Owner.Creature,
            this
        );
        await PowerCmd.Apply<VulnerablePower>(
            cardPlay.Target,
            Owner.Creature.GetPower<YxHexagramPower>().Range(
                RunState,
                DynamicVars["MinVulnerablePower"].IntValue,
                DynamicVars["MaxVulnerablePower"].IntValue,
                out bool _
            ),
            Owner.Creature,
            this
        );
    }
}

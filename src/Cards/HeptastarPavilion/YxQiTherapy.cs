using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Qi Therapy.</summary>
public sealed class YxQiTherapy() : YxCardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain random temporary HP.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxTemporaryHpPower>("MinTemporaryHpPower", 6),
        new PowerVar<YxTemporaryHpPower>("MaxTemporaryHpPower", 14),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <summary>Glow if hexagram is available.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<YxHexagramPower>();

    /// <summary>Gain more temporary HP.</summary>
    protected override void OnUpgrade() => DynamicVars["MaxTemporaryHpPower"].UpgradeValueBy(4);

    /// <summary>Gain random temporary HP.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxTemporaryHpPower>(
            Owner.Creature,
            Owner.Creature.GetPower<YxHexagramPower>()
                .Range(RunState, DynamicVars["MinTemporaryHpPower"].IntValue, DynamicVars["MaxTemporaryHpPower"].IntValue, out bool _),
            Owner.Creature,
            this
        );
    }
}

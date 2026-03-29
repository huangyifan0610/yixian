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

/// <summary>Heptastar Pavilion - Wild Horses Part The Mane.</summary>
public sealed class YxWildHorsesPartTheMane() : YxCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain random block.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar("MinBlock", 6, ValueProp.Move),
        new BlockVar("MaxBlock", 14, ValueProp.Move),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Gains block.</summary>
    public override bool GainsBlock => true;

    /// <summary>Glow if hexagram is enough.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<YxHexagramPower>();

    /// <summary>Gain more block.</summary>
    protected override void OnUpgrade() => DynamicVars["MaxBlock"].UpgradeValueBy(4);

    /// <summary>Gain random block.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        await CreatureCmd.GainBlock(
            Owner.Creature,
            Owner
                .Creature
                .GetPower<YxHexagramPower>()
                .Range(RunState, DynamicVars["MinBlock"].IntValue, DynamicVars["MaxBlock"].IntValue, out bool _),
                ValueProp.Move,
            cardPlay
        );
    }
}

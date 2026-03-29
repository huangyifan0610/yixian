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

/// <summary>Heptastar Pavilion - All Or Nothing.</summary>
public sealed class YxAllOrNothing() : YxCardModel(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Deal random damage; Gain random block.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar("MinDamage", 10m, ValueProp.Move),
        new DamageVar("MaxDamage", 20m, ValueProp.Move),
        new BlockVar("MinBlock", 10m, ValueProp.Move),
        new BlockVar("MaxBlock", 20m, ValueProp.Move),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Gains block.</summary>
    public override bool GainsBlock => true;

    /// <summary>Glow if we have hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPower<YxHexagramPower>()?.Amount >= 2;

    /// <summary>Deal more damage; Gain more block.</summary>
    protected override void OnUpgrade()
    {
        DynamicVars["MinDamage"].UpgradeValueBy(2);
        DynamicVars["MinBlock"].UpgradeValueBy(2);
        DynamicVars["MaxDamage"].UpgradeValueBy(4);
        DynamicVars["MaxBlock"].UpgradeValueBy(4);
    }

    /// <summary>Deal random damage; Gain random block.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));

        decimal damage = Owner
            .Creature
            .GetPower<YxHexagramPower>()
            .Range(RunState, DynamicVars["MinDamage"].IntValue, DynamicVars["MaxDamage"].IntValue, out bool _);
        await DamageCmd
            .Attack(damage)
            .WithHitFx("vfx/vfx_attack_lightning")
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        decimal block = Owner
            .Creature
            .GetPower<YxHexagramPower>()
            .Range(RunState, DynamicVars["MinBlock"].IntValue, DynamicVars["MaxBlock"].IntValue, out bool _);
        await CreatureCmd.GainBlock(Owner.Creature, block, ValueProp.Move, cardPlay);
    }
}

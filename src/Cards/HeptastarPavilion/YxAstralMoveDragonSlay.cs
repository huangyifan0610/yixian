using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Astral Move Dragon Slay.</summary>
public sealed class YxAstralMoveDragonSlay() : YxCardModel(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Astral Move.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.AstralMove];

    /// <summary>Deal damage with bonus from star power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(10),
        new ExtraDamageVar(5),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(CalculatedDamageMultiplyer),
    ];

    /// <summary>Multiplyer for <see cref="CalculatedDamageVar"/>.</summary>
    private static decimal CalculatedDamageMultiplyer(CardModel card, Creature? target) =>
        card.Owner.Creature.GetPower<YxStarPowerPower>()?.Amount ?? 0;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.AstralMove.GetHoverTip(),
        HoverTipFactory.FromPower<YxStarPointPower>(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Glow if on star point.</summary>
    protected override bool ShouldGlowGoldInternal => IsOnStarPoint;

    /// <summary>Can only be played on star point.</summary>
    protected override bool IsPlayable => IsOnStarPoint;

    /// <summary>Gain more bonus from star power.</summary>
    protected override void OnUpgrade() => DynamicVars.ExtraDamage.UpgradeValueBy(1);

    /// <summary>Deal damage with bonus from star power.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(DynamicVars.CalculatedDamage.Calculate(cardPlay.Target))
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }
}

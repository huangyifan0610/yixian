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

/// <summary>Heptastar Pavilion - Astral Move Block.</summary>
public sealed class YxAstralMoveBlock() : YxCardModel(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Astral Move.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.AstralMove];

    /// <summary>Gain block; Gain more block on star point.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(3),
        new CalculationExtraVar(2),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier(CalculatedBlockMultiplyer),
    ];

    /// <summary>Multiplyer for <see cref="CalculatedBlockVar"/>.</summary>
    private static decimal CalculatedBlockMultiplyer(CardModel card, Creature? target) => YxStarPointPower.Test(card)
        ? (card.Owner.Creature.GetPower<YxStarPowerPower>()?.Amount ?? 0)
        : 0;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.AstralMove.GetHoverTip(),
        HoverTipFactory.FromPower<YxStarPointPower>(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Gains block.</summary>
    public override bool GainsBlock => true;

    /// <summary>Glow if on star point.</summary>
    protected override bool ShouldGlowGoldInternal => IsOnStarPoint;

    /// <summary>Gain more blocks.</summary>
    protected override void OnUpgrade() => DynamicVars.CalculationExtra.UpgradeValueBy(1);

    /// <summary>Gain block; Gain more block on star point.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(null), ValueProp.Move, cardPlay);
    }
}

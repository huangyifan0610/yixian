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

/// <summary>Heptastar Pavilion - Astral Move Block.</summary>
public sealed class YxAstralMoveBlock()
    : CardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    , IYxAstralMove
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain block; Gain more block on star point.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(6, ValueProp.Move),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxStarPointPower>(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Glow if on star point.</summary>
    protected override bool ShouldGlowGoldInternal => YxStarPointPower.Test(this);

    /// <summary>Gain more blocks.</summary>
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3);

    /// <summary>Gain block; Gain more block on star point.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Gain block.
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        // Gain more block on star point.
        var starPower = Owner.Creature.GetPower<YxStarPowerPower>();
        if (starPower != null && YxStarPointPower.Test(this))
        {
            await CreatureCmd.GainBlock(Owner.Creature, starPower.Amount, ValueProp.Move, cardPlay);
        }
    }
}

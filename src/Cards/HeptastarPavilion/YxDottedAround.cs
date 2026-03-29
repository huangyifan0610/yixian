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

/// <summary>Heptastar Pavilion - Dotted Around.</summary>
public sealed class YxDottedAround() : YxCardModel(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain block; Gain Star Power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(3, ValueProp.Move),
        new PowerVar<YxStarPowerPower>(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Gains block.</summary>
    public override bool GainsBlock => true;

    /// <summary>Gain more Star Power.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxStarPowerPower)].UpgradeValueBy(1);

    /// <summary>Gain block; Gain Star Power.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<YxStarPowerPower>(
            Owner.Creature,
            DynamicVars[nameof(YxStarPowerPower)].BaseValue,
            Owner.Creature,
            this
        );
    }
}

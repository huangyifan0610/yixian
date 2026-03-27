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

/// <summary>Heptastar Pavilion - Earth Hexagram.</summary>
public sealed class YxEarthHexagram() : YxCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Hexagram.</summary>
    public override IEnumerable<YxCardTag> CanonicalYxTags => [YxCardTag.Hexagram];

    /// <summary>Gain block; Gain hexagram.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(9, ValueProp.Move),
        new PowerVar<YxHexagramPower>(2),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Gains block.</summary>
    public override bool GainsBlock => true;

    /// <summary>Gain more hexagram.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxHexagramPower)].UpgradeValueBy(1);

    /// <summary>Gain block; Gain hexagram.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<YxHexagramPower>(Owner.Creature, DynamicVars[nameof(YxHexagramPower)].BaseValue, Owner.Creature, this);
    }
}

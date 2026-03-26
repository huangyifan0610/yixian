using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Defend.</summary>
public sealed class YxDefendHeptastarPavilion() : CardModel(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain 5 block.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(5, ValueProp.Move),
    ];

    /// <summary>Defend.</summary>
    public override IEnumerable<CardTag> Tags => [CardTag.Defend];

    /// <summary>Gain more block.</summary>
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3);

    /// <summary>Gain block.</summary>
    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
}

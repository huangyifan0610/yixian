using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// <c>Earch Hexagram</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class EarthHexagram() : HeptastarPavilionCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    /// <summary>
    /// Variable name of Hexagram.
    /// </summary>
    private const string HEXAGRAM_VAR = "Hexagram";

    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Gain 4 blocks.
        new BlockVar(4, ValueProp.Move),
        // Gain 2 hexagrams.
        new IntVar(HEXAGRAM_VAR, 2),
    ]);

    /// <summary>
    /// Adds star point power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<HexagramPower>(),
    ]);

    /// <summary>
    /// Gain blocks and hexagrams.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Gain blocks.
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        // Gain hexagrams.
        await PowerCmd.Apply<HexagramPower>([Owner.Creature], DynamicVars[HEXAGRAM_VAR].IntValue, Owner.Creature, this);
    }

    /// <summary>
    /// Upgrade the block.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
        DynamicVars[HEXAGRAM_VAR].UpgradeValueBy(1);
    }
}
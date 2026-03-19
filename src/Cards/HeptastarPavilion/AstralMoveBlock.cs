using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Commands;
using System.Linq;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// <c>Astral Move - Block</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class AstralMoveBlock : StarPointCardModel
{
    /// <summary>
    /// The star point block variable.
    /// </summary>
    private const string STAR_POINT_BLOCK_VAR = "StarPointBlock";

    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Gain 5 blocks.
        new BlockVar(5, ValueProp.Move),
        // Star Point: Gain 5 blocks.
        new BlockVar(STAR_POINT_BLOCK_VAR, 3, ValueProp.Move),
    ]);

    /// <summary>
    /// The card tags.
    /// </summary>
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    /// <summary>
    /// The default constructor.
    /// </summary>
    public AstralMoveBlock() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self) { }

    /// <summary>
    /// Gain blocks.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Gain block.
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        // Continue to gain star point block.
        if (IsStarPointVar.BoolVal)
        {
            await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars[STAR_POINT_BLOCK_VAR], cardPlay);

            // Reset star point.
            IsStarPointVar.BoolVal = false;
        }
    }

    /// <summary>
    /// Upgrade the block.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(1);
        DynamicVars[STAR_POINT_BLOCK_VAR].UpgradeValueBy(3);
    }
}
